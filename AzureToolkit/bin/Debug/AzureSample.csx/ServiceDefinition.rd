<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="AzureSample" generation="1" functional="0" release="0" Id="3db6e520-7933-4efa-a648-fc236a541e5f" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="AzureSampleGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="DBDCRole:HttpIn" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/AzureSample/AzureSampleGroup/LB:DBDCRole:HttpIn" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="DBDCRoleInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/AzureSample/AzureSampleGroup/MapDBDCRoleInstances" />
          </maps>
        </aCS>
        <aCS name="DBDCRole:DiagnosticsConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/AzureSample/AzureSampleGroup/MapDBDCRole:DiagnosticsConnectionString" />
          </maps>
        </aCS>
        <aCS name="DBDCRole:DbDotComConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/AzureSample/AzureSampleGroup/MapDBDCRole:DbDotComConnectionString" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:DBDCRole:HttpIn">
          <toPorts>
            <inPortMoniker name="/AzureSample/AzureSampleGroup/DBDCRole/HttpIn" />
          </toPorts>
        </lBChannel>
      </channels>
      <maps>
        <map name="MapDBDCRoleInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/AzureSample/AzureSampleGroup/DBDCRoleInstances" />
          </setting>
        </map>
        <map name="MapDBDCRole:DiagnosticsConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/AzureSample/AzureSampleGroup/DBDCRole/DiagnosticsConnectionString" />
          </setting>
        </map>
        <map name="MapDBDCRole:DbDotComConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/AzureSample/AzureSampleGroup/DBDCRole/DbDotComConnectionString" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="DBDCRole" generation="1" functional="0" release="0" software="C:\Users\Josh\Documents\Visual Studio 2010\Projects\AzureSample\AzureSample\bin\Debug\AzureSample.csx\roles\DBDCRole" entryPoint="base\x86\WaWebHost.exe" parameters="" memIndex="1792" hostingEnvironment="frontendfulltrust" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="HttpIn" protocol="http" />
            </componentports>
            <settings>
              <aCS name="DiagnosticsConnectionString" defaultValue="" />
              <aCS name="DbDotComConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;DBDCRole&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;DBDCRole&quot;&gt;&lt;e name=&quot;HttpIn&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/AzureSample/AzureSampleGroup/DBDCRoleInstances" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyID name="DBDCRoleInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="b63ac61a-67c8-45bb-ae1f-b51e7a2d7f23" ref="Microsoft.RedDog.Contract\ServiceContract\AzureSampleContract@ServiceDefinition">
      <interfacereferences>
        <interfaceReference Id="8296e750-edf5-426e-b7f8-704001cc0704" ref="Microsoft.RedDog.Contract\Interface\DBDCRole:HttpIn@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/AzureSample/AzureSampleGroup/DBDCRole:HttpIn" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>