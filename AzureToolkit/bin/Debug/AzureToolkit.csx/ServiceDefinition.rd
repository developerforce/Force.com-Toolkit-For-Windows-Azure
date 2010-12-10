<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="AzureSample" generation="1" functional="0" release="0" Id="17fefb2f-9d10-46b4-858d-a8ad917f7393" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
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
          <role name="DBDCRole" generation="1" functional="0" release="0" software="C:\Users\Josh\Documents\Visual Studio 2010\Projects\AzureToolkit\AzureToolkit\bin\Debug\AzureToolkit.csx\roles\DBDCRole" entryPoint="base\x86\WaWebHost.exe" parameters="" memIndex="1792" hostingEnvironment="frontendfulltrust" hostingEnvironmentVersion="2">
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
    <implementation Id="68060b33-dc12-40b1-b46a-cd024da46de4" ref="Microsoft.RedDog.Contract\ServiceContract\AzureSampleContract@ServiceDefinition">
      <interfacereferences>
        <interfaceReference Id="0d273697-beee-41f3-b37c-9ba216fc2a01" ref="Microsoft.RedDog.Contract\Interface\DBDCRole:HttpIn@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/AzureSample/AzureSampleGroup/DBDCRole:HttpIn" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>