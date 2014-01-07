using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DatabaseDotCom;
using System.Data;
using System.Collections;

namespace DatabaseDotCom
{
    public class DatabaseDotComClient
    {
        private DBDCReference.SoapClient soapclient;
        public DatabaseDotComContext context { get; set; }

        public DatabaseDotComClient()
            : base()
        {
            soapclient = new DBDCReference.SoapClient("Soap");
        }

        public DatabaseDotComClient(DatabaseDotComContext context) 
        {
            soapclient = new DBDCReference.SoapClient("Soap");
            if (context.loginResult == null)
            {
                Login(context);
            }
            soapclient = new DBDCReference.SoapClient("Soap", context.loginResult.serverUrl);
            this.context = context;
        }

        protected void Login(DatabaseDotComContext context)
        {
            System.Diagnostics.Debug.WriteLine("\nLOGGING IN\n");
            context.loginResult = soapclient.login(context.loginScopeHeader, context.callOptions, context.username, context.password);
            context.sessionHeader = new DBDCReference.SessionHeader();
            context.sessionHeader.sessionId = context.loginResult.sessionId;
        }

        public DatabaseDotComContext Login(string userName, string password, string authToken)
        {
            context = new DatabaseDotComContext(userName, password, authToken);
            context.loginResult = soapclient.login(context.loginScopeHeader, context.callOptions, context.username, context.password);
            context.sessionHeader = new DBDCReference.SessionHeader();
            context.sessionHeader.sessionId = context.loginResult.sessionId;
            return context;
        }

        public QueryResult Query(string soql)
        {
            return new QueryResult(soapclient.query(context.sessionHeader, context.callOptions, context.queryOptions, context.mruHeader, context.packageVersionHeader, soql));
        }
        public QueryResult QueryAll(string soql)
        {
            return new QueryResult(soapclient.queryAll(context.sessionHeader, context.callOptions, context.queryOptions, soql));
        }
        public QueryResult QueryMore(string queryLocator) {
            return new QueryResult(soapclient.queryMore(context.sessionHeader, context.callOptions,
                context.queryOptions, queryLocator));
        }
        public DBDCReference.DeleteResult[] DeleteByIds(string[] ids)
        {
            DBDCReference.DeleteResult[] deleteresults;
            DBDCReference.DebuggingInfo result =  soapclient.delete(context.sessionHeader, context.callOptions, context.packageVersionHeader,
                context.userTerritoryDeleteHeader, context.emailHeader, context.allowFieldTruncationHeader,
                context.disableFeedTrackingHeader, context.allOrNoneHeader, context.debuggingHeader,
                ids, out deleteresults);
            return deleteresults;
        }
        public DBDCReference.DeleteResult[] DeleteObjects(DatabaseDotComClient.DBCObject[] objects)
        {
            string[] ids = new string[objects.Length];
            for (int i = 0; i < ids.Length; i++)
            {
                ids[i] = objects[i].Id;
            }
            return DeleteByIds(ids);
        }
        public DBDCReference.DeleteResult[] DeleteByQueryResult(DatabaseDotComClient.QueryResult queryresult)
        {
            return DeleteObjects(queryresult.records);
        }
        public DBCObject[] Update(DBCObject[] newObjects, out DBDCReference.SaveResult[] result)
        {
            return CreateUpdate(newObjects, "Update", out result);
        }
        public DBCObject Update(DBCObject updateObject, out DBDCReference.SaveResult result)
        {
            return CreateUpdate(updateObject, "Update", out result);
        }
        public DBCObject Create(DBCObject newObject, out DBDCReference.SaveResult result)
        {
            return CreateUpdate(newObject, "Create", out result);
        }
        public DBCObject[] Create(DBCObject[] newObjects, out DBDCReference.SaveResult[] result)
        {
            return CreateUpdate(newObjects, "Create", out result);
        }

        private DBCObject CreateUpdate(DBCObject newObject, string op, out DBDCReference.SaveResult result)
        {
            DBDCReference.SaveResult[] results;
            DBCObject[] savedObjects = this.CreateUpdate(new DBCObject[] { newObject }, op, out results);
            result = results[0];
            newObject.Id = result.id;
            return newObject;
        }
        private DBCObject[] CreateUpdate(DBCObject[] newObjects, string op, out DBDCReference.SaveResult[] result)
        {
            DBDCReference.sObject[] _newObjects = new DBDCReference.sObject[newObjects.Length];
            for (int i = 0; i < newObjects.Length; i++)
                _newObjects[i] = newObjects[i].SoapObject;
            if (op.Equals("Create")) {
            soapclient.create(context.sessionHeader, context.callOptions, context.assignmentRuleHeader,
                context.mruHeader, context.allowFieldTruncationHeader, context.disableFeedTrackingHeader, context.allOrNoneHeader,
                context.debuggingHeader, context.packageVersionHeader, context.emailHeader, _newObjects, out result);
            } else {
            soapclient.update(context.sessionHeader, context.callOptions, context.assignmentRuleHeader,
                context.mruHeader, context.allowFieldTruncationHeader, context.disableFeedTrackingHeader, context.allOrNoneHeader,
                context.debuggingHeader, context.packageVersionHeader, context.emailHeader, _newObjects, out result);
            }
            for (int i = 0; i < result.Length; i++)
            {
                newObjects[i].Id = result[i].id;
//                newObjects[i].SetStringField("Id", result[i].id);
            }

            return newObjects;
        }


        #region QueryResult embedded class
        public class QueryResult
        {
            public int size { get; set; }
            public bool done { get; set; }
            public string queryLocator { get; set; }
            public DBCObject[] records { get; set; }

            public QueryResult() { }
            public QueryResult(DatabaseDotCom.DBDCReference.QueryResult qr)
            {
                this.size = qr.size;
                this.done = qr.done;
                this.queryLocator = qr.queryLocator;
                this.records = MakeRecords(qr.records);
            }

            public QueryResult(System.Xml.XmlNode node)
            {
                List<DBCObject> recs = new List<DBCObject>();
                foreach (System.Xml.XmlNode childNode in node.ChildNodes)
                {
                    if (childNode.LocalName.Equals("size"))
                    {
                        this.size = int.Parse(childNode.InnerText);
                    }
                    else if (childNode.LocalName.Equals("queryLocator"))
                    {
                        this.queryLocator = childNode.InnerText;
                    }
                    else if (childNode.LocalName.Equals("done"))
                    {
                        this.done = bool.Parse(childNode.InnerText);
                    }
                    else if (childNode.LocalName.Equals("records"))
                    {
                        recs.Add(MakeRecord(childNode));
                    }
                }
                this.records = recs.ToArray();
            }
            private DBCObject MakeRecord(System.Xml.XmlNode node)
            {
                return new DBCObject(node);
            }
            private DBCObject[] MakeRecords(DBDCReference.sObject[] sObjects)
            {
                List<DBCObject> objs = new List<DBCObject>();

                if (sObjects != null)
                {
                    foreach (DBDCReference.sObject obj in sObjects)
                    {
                        objs.Add(new DBCObject(obj));
                    }
                }

                return objs.ToArray();
            }
            public DataSet GetDataSet()
            {
                DataSet ds = new DataSet();
                if (records != null)
                {
                    ds.Tables.Add(toDataTable());
                }
                else
                {
                    ds.Tables.Add(new DataTable());
                }
                return ds;
            }
            private DataTable toDataTable()
            {
                DataTable dt = new DataTable();
                foreach (DBCObject obj in records)
                {
                    Dictionary<string, object> fields = obj.getFields();
                    foreach (string key in fields.Keys)
                    {
                        if (fields[key].GetType().Equals(typeof(string)))
                        {
                            if (!dt.Columns.Contains(key))
                            {
                                dt.Columns.Add(new DataColumn(key));
                            }
                        }
                    }
                    dt.Rows.Add(obj.SimpleDataRow(dt));
                }
                return dt;
            }
        }
        #endregion

        #region DBCsObject embedded class
        public class DBCObject
        {
            private DBDCReference.sObject _sobject;
            private string _type;
            private ArrayList _any;
            private string[] _fieldsToNull;
            private string _id;
            private Dictionary<string, object> _fields;
            private Dictionary<string, int> _anyIndexer;
            private System.Xml.XmlNode node;

            public DBDCReference.sObject SoapObject
            {
                get
                {
                    SyncSoapObjectToFieldMap();
                    return _sobject; 
                }
            }

            private void SyncSoapObjectToFieldMap()
            {
                int fieldCount = _fields.Count;
                if (_fields.ContainsKey("Id") || _fields.ContainsKey("id")
                    || _fields.ContainsKey("iD") || _fields.ContainsKey("ID") ) {
                    fieldCount = fieldCount - 1;
                }
                _sobject.Any = new System.Xml.XmlElement[fieldCount];
                _any = new ArrayList();
                int index = 0;
                foreach (string key in _fields.Keys)
                {
                    if (!key.ToLower().Equals("id"))
                    {
                        _sobject.Any[index] = MakeXmlElement(key, _fields[key].ToString());
                        index++;
                    }
                }
                _any.AddRange(_sobject.Any);
            }
            private String getField(String fieldname)
            {
                if (_fields.ContainsKey(fieldname))
                {
                    return _fields[fieldname].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
            public Dictionary<string, object> getFields()
            {
                return _fields;
            }
            public String GetStringField(string fieldname)
            {
                return getField(fieldname);
            }
            public void SetStringField(string fieldname, string fieldvalue)
            {
                if (_fields.ContainsKey(fieldname))
                {
                    _fields[fieldname] = fieldvalue;
                }
                else
                {
                    _fields.Add(fieldname, fieldvalue);
                }
                AddToAny(fieldname, _fields[fieldname].ToString());
            }
            public double GetNumberField(string fieldname)
            {
                string fieldValue = getField(fieldname);
                if (!fieldValue.Equals(string.Empty))
                {
                    return Double.Parse(fieldValue);
                }
                else
                {
                    return Double.NaN;
                }
            }
            public void SetNumberField(string fieldname, Double fieldvalue)
            {
                if (_fields.ContainsKey(fieldname))
                {
                    _fields[fieldname] = fieldvalue.ToString();
                }
                else
                {
                    _fields.Add(fieldname, fieldvalue.ToString());
                }
                AddToAny(fieldname, _fields[fieldname].ToString());
            }
            public void SetNumberField(string fieldname, int fieldvalue)
            {
                if (_fields.ContainsKey(fieldname))
                {
                    _fields[fieldname] = fieldvalue.ToString();
                }
                else
                {
                    _fields.Add(fieldname, fieldvalue.ToString());
                }
                AddToAny(fieldname, _fields[fieldname].ToString());
            }
            public bool GetBooleanField(string fieldname)
            {
                string fieldValue = getField(fieldname);
                if (!fieldValue.Equals(string.Empty))
                {
                    return bool.Parse(fieldValue);
                }
                else
                {
                    return false;
                }
            }
            public void SetBooleanField(string fieldname, bool fieldvalue)
            {
                if (_fields.ContainsKey(fieldname))
                {
                    _fields[fieldname] = fieldvalue.ToString();
                }
                else
                {
                    _fields.Add(fieldname, fieldvalue.ToString());
                }
                AddToAny(fieldname, _fields[fieldname].ToString());
            }
            public DateTime GetDateTimeField(string fieldname)
            {
                string fieldvalue = getField(fieldname);
                if (!fieldvalue.Equals(string.Empty))
                {
                    return DateTime.Parse(fieldvalue);
                }
                else
                {
                    throw new InvalidCastException("Field " + fieldname + " not found in sObject.");
                }
            }
            public void SetDateTimeField(string fieldname, DateTime fieldvalue)
            {
                if (_fields.ContainsKey(fieldname))
                {
                    _fields[fieldname] = System.Xml.XmlConvert.ToString(fieldvalue, System.Xml.XmlDateTimeSerializationMode.Local);
                }
                else
                {
                    _fields.Add(fieldname, System.Xml.XmlConvert.ToString(fieldvalue, System.Xml.XmlDateTimeSerializationMode.Local));
                }
                AddToAny(fieldname, _fields[fieldname].ToString());
            }

            private void AddToAny(string fieldname, string fieldvalue, int indexer)
            {
                if (_any.Count == indexer)
                {
                    _any.Add(MakeXmlElement(fieldname, fieldvalue));
                }
                else
                {
                    _any[indexer] = MakeXmlElement(fieldname, fieldvalue);
                }
                ConvertAny();
            }
            private void AddToAny(string fieldname, string fieldvalue)
            {
                if (_anyIndexer.ContainsKey(fieldname))
                {
                    AddToAny(fieldname, _fields[fieldname].ToString(), _anyIndexer[fieldname]);
                }
                else
                {
                    AddToAny(fieldname, fieldvalue, _any.Count);
                }
            }
            private void ConvertAny()
            {
                _sobject.Any = new System.Xml.XmlElement[_any.Count];
                for (int i = 0; i < _any.Count; i++)
                    _sobject.Any[i] = (System.Xml.XmlElement)_any[i];
            }
            public static System.Xml.XmlElement MakeXmlElement(string Name, string nodeValue)
            {
                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                System.Xml.XmlElement xmlel = doc.CreateElement(Name);
                xmlel.InnerText = nodeValue;
                return xmlel;
            }

            private DBDCReference.sObject sobject
            {
                set
                {
                    _sobject = value;
                    type = value.type;
                    _any.AddRange(value.Any);
                    fieldsToNull = value.fieldsToNull;
                    Id = value.Id;
                }
            }
            public string[] fieldsToNull
            {
                get
                {
                    return _fieldsToNull;
                }
                set { _fieldsToNull = value; }
            }
            public string Id
            {
                get
                {
                    return _id;
                }
                set
                {
                    _id = value;
                    _sobject.Id = value;
                }
            }
            public string type
            {
                get
                {
                    return _type;
                }
                set { _type = value; }
            }
            private void init(DBDCReference.sObject sobject)
            {
                _anyIndexer = new Dictionary<string, int>();
                _fields = new Dictionary<string, object>();
                _any = new ArrayList();
                if (sobject == null)
                {
                    _sobject = new DBDCReference.sObject();
                }
                else
                {
                    _sobject = sobject;
                }
            }
            public DBCObject(string type)
            {
                this.type = type;
                init(null);
                _sobject.type = type;
            }
            public DBCObject(DBDCReference.sObject sobject)
            {
                init(sobject);
                initObject();
            }

            public DBCObject(System.Xml.XmlNode node)
            {
                init(null);
                List<System.Xml.XmlElement> anys = new List<System.Xml.XmlElement>();

                foreach (System.Xml.XmlNode n in node.ChildNodes)
                {
                    if (n.LocalName.Equals("Id"))
                    {
                        _sobject.Id = n.InnerText;
                    }
                    else if (n.LocalName.Equals("type"))
                    {
                        _sobject.type = n.InnerText;
                    }
                    else if (n.LocalName.Equals("fieldsToNull"))
                    {
                        //_sobject.fieldsToNull = n.InnerText;
                    }
                    else
                    {
                        anys.Add(MakeXmlElement(n.LocalName, n.InnerText));
                    }
                }
                _sobject.Any = anys.ToArray();
                _fields = new Dictionary<string, object>();
                initObject();
            }

            private void initObject()
            {
                this.Id = _sobject.Id;
                this.type = _sobject.type;
                this.fieldsToNull = _sobject.fieldsToNull;
                this._fields = new Dictionary<string, object>();
                foreach (System.Xml.XmlElement el in _sobject.Any)
                {
                    if (el.Attributes.Count > 0 && (el.Attributes[0].Name.Equals("xsi:type") && el.Attributes[0].InnerText.Equals("QueryResult")))
                    {
                        this._fields.Add(el.LocalName, ToQueryResult(el));
                    }
                    else if (el.Attributes.Count > 0 && (el.Attributes[0].Name.Equals("xsi:type") && el.Attributes[0].InnerText.Equals("sf:sObject")))
                    {
                        this._fields.Add(el.LocalName, new DBCObject(el));
                    }
                    else if (el.Attributes.Count > 0 && (el.Attributes[0].Name.Equals("xsi:nil") && el.Attributes[0].InnerText.Equals("true")))
                    {
                        this._fields.Add(el.LocalName, new object());
                    }
                    else
                    {
                        this._fields.Add(el.LocalName, el.InnerText);
                    }
                }
            }

            private QueryResult ToQueryResult(System.Xml.XmlNode node)
            {
                System.Diagnostics.Debug.WriteLine(node.LocalName);
                return new QueryResult(node);
            }
            //private sObject TosObject() {
            public DataRow SimpleDataRow(DataTable dt)
            {
                DataRow dr = dt.NewRow();
                foreach (DataColumn dc in dt.Columns)
                {
                    if (_fields[dc.ColumnName].GetType().Equals(typeof(string)))
                    {
                        dr[dc.ColumnName] = _fields[dc.ColumnName];
                    }
                }
                return dr;
            }

        }
        #endregion

    }
}
