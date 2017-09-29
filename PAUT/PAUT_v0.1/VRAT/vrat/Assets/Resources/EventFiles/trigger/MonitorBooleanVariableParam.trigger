<vrat.TriggerPrimitivesTemplate name="MonitorBooleanVariableParam" type="TriggerPrimitivesTemplate">
  <PairwiseAsset name="OwnedAssetName" type="STRING" contents="" />
  <PossibleAttachedAssetType name="AttachedAssetType" type="STRING" contents="" />
  <CustomizedProperty>
    <vrat.ListOfXmlTemplate name="PropertiesList" type="ListOfXmlTemplate" idx="0">
      <vrat.ListOfXmlTemplate name="TriggerType" type="CHOICE" idx="0">
        <vrat.PrimitiveXmlTemplate name="TriggerType0" type="STRING" contents="MonitorValueCaptured" />
      </vrat.ListOfXmlTemplate>
      <vrat.PrimitiveXmlTemplate name="Threshold" type="BOOL" contents="True" />
      <vrat.PrimitiveXmlTemplate name="VariableName" type="STRING" contents="" />
    </vrat.ListOfXmlTemplate>
  </CustomizedProperty>
</vrat.TriggerPrimitivesTemplate>