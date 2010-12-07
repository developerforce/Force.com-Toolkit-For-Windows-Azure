using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DatabaseDotCom.DBDCReference;
using System.Web.Configuration;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace DatabaseDotCom
{
    public class DatabaseDotComContext
    {
        public AllOrNoneHeader allOrNoneHeader { get; set; }
        public AllowFieldTruncationHeader allowFieldTruncationHeader { get; set; }
        public AssignmentRuleHeader assignmentRuleHeader { get; set; }
        public DebuggingHeader debuggingHeader { get; set; }
        public DisableFeedTrackingHeader disableFeedTrackingHeader { get; set; }
        public EmailHeader emailHeader { get; set; }
        public LoginScopeHeader loginScopeHeader { get; set; }
        public MruHeader mruHeader { get; set; }
        public SessionHeader sessionHeader { get; set; }
        public UserTerritoryDeleteHeader userTerritoryDeleteHeader { get; set; }
        public CallOptions callOptions { get; set; }
        public QueryOptions queryOptions { get; set; }
        public PackageVersion[] packageVersionHeader { get; set; }
        public LoginResult loginResult { get; set; }

        public string username { get; set; }
        private string _password;
        private string configConnectionString;
        public string loginEnpoint { get; set; }
        public string token { get; set; }


        public string password
        {
            get
            {
                if (token.Equals(string.Empty) || token == null)
                {
                    return _password;
                }
                else
                {
                    return _password + token;
                }
            }
            set
            {
                _password = value;
            }
        }

        public DatabaseDotComContext()
        {
        }
        public static DatabaseDotComContext FromConfigurationSetting(String connectionString)
        {
            try
            {
                String configConnectionString = RoleEnvironment.GetConfigurationSettingValue(connectionString);
                System.Diagnostics.Debug.WriteLine("Connection string is: " + configConnectionString);
                return new DatabaseDotComContext(configConnectionString);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Error reading config...");
                return null;
                //ConfigId.Text = e.ToString();
            }
        }

        public DatabaseDotComContext(string username, string password)
        {
        }

        public DatabaseDotComContext(string username, string password, string authtoken)
        {
            this.username = username;
            this.token = authtoken;
            this.password = password;
        }

        public DatabaseDotComContext(string configConnectionString)
        {
            this.configConnectionString = configConnectionString;
            parseConnectionString(this.configConnectionString);
            DatabaseDotComClient c = new DatabaseDotComClient();
            DatabaseDotComContext ct = c.Login(this.username, _password, token);
            this.sessionHeader = ct.sessionHeader;
            this.allOrNoneHeader = ct.allOrNoneHeader;
            this.allowFieldTruncationHeader = ct.allowFieldTruncationHeader;
            this.assignmentRuleHeader = ct.assignmentRuleHeader;
            this.callOptions = ct.callOptions;
            this.debuggingHeader = ct.debuggingHeader;
            this.disableFeedTrackingHeader = ct.disableFeedTrackingHeader;
            this.emailHeader = ct.emailHeader;
            this.loginEnpoint = ct.loginEnpoint;
            this.loginResult = ct.loginResult;
            this.loginScopeHeader = ct.loginScopeHeader;
            this.mruHeader = ct.mruHeader;
            this.packageVersionHeader = ct.packageVersionHeader;
            this.token = ct.token;
            this.password = ct.password;
            this.queryOptions = ct.queryOptions;
            this.username = ct.username;
            this.userTerritoryDeleteHeader = ct.userTerritoryDeleteHeader;
           
        }

        private void parseConnectionString(String connectionString)
        {
            String[] pairs = connectionString.Split(";".ToCharArray());
            username = getValue(pairs[0], "=");
            password = getValue(pairs[1], "=");
            this.token = getValue(pairs[2], "=");
            this.loginEnpoint = getValue(pairs[3], "=");
        }
        private String getValue(String kv, String sep)
        {
            String[] kp = kv.Split(sep.ToCharArray());
            if (kp.Length > 1)
            {
                return kp[1];
            }
            else
            {
                return String.Empty;
            }
        }
    }
}
