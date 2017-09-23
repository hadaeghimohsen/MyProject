using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace System.Reporting.ReportProfiler.Ui
{
   partial class SpecifyFilter
   {
      private XElement xData;

      public XElement XData
      {
         get { return xData; }
         set {
            xData = value;
         }
      }

      #region Filter Xml Template
      private XDocument xTemplate = XDocument.Parse(@"
<FilterTemplate>
	<Type name=""int"">
		<Controls action=""enabling"">
			<Control tag=""0""> <!-- Schema CheckBox -->
				<Select case=""false"">
					<Controls action=""disabling"">
						<Control tag=""1""/>
						<Control tag=""2""/>
						<Control tag=""3""/>
						<Control tag=""4""/>
						<Control tag=""5""/>
						<Control tag=""6""/>
						<Control tag=""7""/>
						<Control tag=""8""/>
						<Control tag=""9""/>
						<Control tag=""11""/>
						<Control tag=""13""/>
						<Control tag=""25""/>
						<Control tag=""26""/>
					</Controls>
					<Controls action=""enabling""/>
				</Select>
				<Select case=""true"">
					<Controls action=""enabling"">
						<Control tag=""1""/>
						<Control tag=""2""/>
						<Control tag=""3""/>
						<Control tag=""4""/>
						<Control tag=""5""/>
						<Control tag=""6""/>
						<Control tag=""7""/>
						<Control tag=""8""/>
						<Control tag=""9""/>
						<Control tag=""11""/>
						<Control tag=""13""/>
						<Control tag=""25""/>
						<Control tag=""26""/>
					</Controls>					
					<Controls action=""disabling""/>
				</Select>
			</Control>
			<Control tag=""2""> <!-- High Access Control ComboBox -->
				<Select case=""0"">
					<Controls action=""enabling"">
						<Control tag=""7""/>
						<Control tag=""8""/>
						<Control tag=""9""/>
						<Control tag=""11""/>
						<Control tag=""13""/>
						<Control tag=""16""/>
						<Control tag=""17""/>
						<Control tag=""19""/>
						<Control tag=""21""/>
						<Control tag=""23""/>
						<Control tag=""25""/>
						<Control tag=""26""/>						
					</Controls>
					<Controls action=""disabling""/>						
				</Select>
				<Select case=""1"">
					<Controls action=""enabling"">
						<Control tag=""7""/>
						<Control tag=""8""/>
						<Control tag=""9""/>
						<Control tag=""11""/>
						<Control tag=""13""/>
						<Control tag=""16""/>
						<Control tag=""17""/>
						<Control tag=""19""/>
						<Control tag=""21""/>
						<Control tag=""23""/>
						<Control tag=""25""/>
						<Control tag=""26""/>			
					</Controls>
					<Controls action=""disabling""/>
				</Select>
				<Select case=""2"">
					<Controls action=""enabling"">
						<Control tag=""7""/>
						<Control tag=""8""/>
						<Control tag=""9""/>
						<Control tag=""11""/>
						<Control tag=""13""/>						
						<Control tag=""21""/>
						<Control tag=""23""/>
						<Control tag=""25""/>
						<Control tag=""26""/>						
					</Controls>
					<Controls action=""disabling"">						
						<Control tag=""16""/>
						<Control tag=""17""/>
						<Control tag=""19""/>						
					</Controls>					
				</Select>
			</Control>
			<Control tag=""3""/> <!-- Table/Domain -->
			<Control tag=""4""/> <!-- Column Code -->
			<Control tag=""5""/> <!-- Column Value -->
			<Control tag=""6""/> <!-- Show Meaning -->
			<Control tag=""7""/> <!-- Select Option -->
			<Control tag=""8""/> <!-- Distinct -->
			<Control tag=""9""/> <!-- Default Values CheckBox -->
			<Control tag=""11""/> <!-- RowNum Or Top CheckBox -->
			<Control tag=""13""/> <!-- Order By CheckBox -->
			<Control tag=""16""/> <!-- Digit Seprator -->
			<Control tag=""17""/> <!-- Max Len CheckBox -->
			<Control tag=""19""/> <!-- Number Point CheckBox -->
			<Control tag=""21""/> <!-- Min Value CheckBox -->
			<Control tag=""23""/> <!-- Max Value CheckBox -->				
			<Control tag=""25""/> <!-- NULLS VALUE -->
			<Control tag=""26""/> <!-- Where CheckBox -->				
		</Controls>
		<Controls action=""disabling"">
			<Control tag=""15""/>
		</Controls>
	</Type>
	<Type name=""string"">
		<Controls action=""enabling"">
			<Control tag=""0"">			
				<Select case=""false"">
					<Controls action=""disabling"">
						<Control tag=""1""/> <!-- Schema Combo -->
						<Control tag=""2""/> <!-- High Access Control -->
						<Control tag=""3""/> <!-- Table/Domain -->
						<Control tag=""4""/> <!-- Column Code -->
						<Control tag=""5""/> <!-- Column Value -->
						<Control tag=""6""/> <!-- Show Meaning -->
						<Control tag=""7""/> <!-- Select Option -->
						<Control tag=""8""/> <!-- Distinct -->
						<Control tag=""9""/> <!-- Default Values -->
						<Control tag=""11""/> <!-- RowNum -->
						<Control tag=""13""/> <!-- Order By -->
						<Control tag=""25""/> <!-- IS NOT NULLS -->
						<Control tag=""26""/> <!-- Where -->
					</Controls>
					<Controls action=""enabling""/>
				</Select>
				<Select case=""true"">
					<Controls action=""enabling"">
						<Control tag=""1""/> <!-- Schema Combo -->
						<Control tag=""2""/> <!-- High Access Control -->
						<Control tag=""3""/> <!-- Table/Domain -->
						<Control tag=""4""/> <!-- Column Code -->
						<Control tag=""5""/> <!-- Column Value -->
						<Control tag=""6""/> <!-- Show Meaning -->
						<Control tag=""7""/> <!-- Select Option -->
						<Control tag=""8""/> <!-- Distinct -->
						<Control tag=""9""/> <!-- Default Values -->
						<Control tag=""11""/> <!-- RowNum -->
						<Control tag=""13""/> <!-- Order By -->
						<Control tag=""25""/> <!-- IS NOT NULLS -->
						<Control tag=""26""/> <!-- Where -->
					</Controls>
					<Controls action=""disabling""/>
				</Select>
			</Control>
			<Control tag=""2"">
				<Select case=""0"">
					<Controls action=""enabling"">
						<Control tag=""7""/>
						<Control tag=""8""/>
						<Control tag=""9""/>
						<Control tag=""11""/>
						<Control tag=""13""/>
						<Control tag=""15""/>
						<Control tag=""17""/>
						<Control tag=""25""/>
						<Control tag=""26""/>
					</Controls>
					<Controls action=""disabling""/>
				</Select>
				<Select case=""1"">
					<Controls action=""enabling"">
						<Control tag=""7""/>
						<Control tag=""8""/>
						<Control tag=""9""/>
						<Control tag=""11""/>
						<Control tag=""13""/>
						<Control tag=""15""/>
						<Control tag=""17""/>
						<Control tag=""25""/>
						<Control tag=""26""/>
					</Controls>
					<Controls action=""disabling""/>					
				</Select>
				<Select case=""2"">
					<Controls action=""enabling"">
						<Control tag=""7""/>
						<Control tag=""8""/>
						<Control tag=""9""/>
						<Control tag=""11""/>
						<Control tag=""13""/>
						<Control tag=""15""/>
						<Control tag=""17""/>
						<Control tag=""25""/>
						<Control tag=""26""/>
					</Controls>
					<Controls action=""disabling""/>
				</Select>
			</Control>
			<Control tag=""3""/>
			<Control tag=""4""/>
			<Control tag=""5""/>
			<Control tag=""6""/>
			<Control tag=""7""/>
			<Control tag=""8""/>
			<Control tag=""9""/>
			<Control tag=""11""/>
			<Control tag=""13""/>
			<Control tag=""15"">
				<Select case=""false"">
					<Controls action=""enabling"">
						<Control tag=""0""/>
						<Control tag=""2""/>
						<Control tag=""3""/>
						<Control tag=""4""/>
						<Control tag=""5""/>
						<Control tag=""6""/>
						<Control tag=""7""/>
						<Control tag=""8""/>
						<Control tag=""9""/>
						<Control tag=""11""/>
						<Control tag=""13""/>
						<Control tag=""25""/>
						<Control tag=""26""/>
					</Controls>
					<Controls action=""disabling"">						
						<Control tag=""21""/>
						<Control tag=""23""/>
					</Controls>
				</Select>
				<Select case=""true"">
					<Controls action=""enabling"">
						<Control tag=""21""/>
						<Control tag=""23""/>
					</Controls>
					<Controls action=""disabling"">
						<Control tag=""0""/>
						<Control tag=""2""/>
						<Control tag=""3""/>
						<Control tag=""4""/>
						<Control tag=""5""/>
						<Control tag=""6""/>
						<Control tag=""7""/>
						<Control tag=""8""/>
						<Control tag=""9""/>
						<Control tag=""11""/>
						<Control tag=""13""/>
						<Control tag=""25""/>
						<Control tag=""26""/>
					</Controls>
				</Select>
			</Control>
			<Control tag=""17""/>
			<Control tag=""25""/>
			<Control tag=""26""/>
		</Controls>
		<Controls action=""disabling"">
			<Control tag=""16""/>
			<Control tag=""19""/>
			<Control tag=""21""/>
			<Control tag=""23""/>
		</Controls>
	</Type>
	<Type name=""stringdatetime"">
		<Controls action=""enabling"">
			<Control tag=""15"">
				<Select case=""false"">
					<Controls action=""enabling"">
						<Control tag=""0""/>
						<Control tag=""1""/>
						<Control tag=""2""/>
						<Control tag=""3""/>
						<Control tag=""4""/>
						<Control tag=""5""/>
						<Control tag=""6""/>
						<Control tag=""7""/>
						<Control tag=""8""/>
						<Control tag=""9""/>
						<Control tag=""11""/>
						<Control tag=""13""/>
						<Control tag=""25""/>
						<Control tag=""26""/>
					</Controls>
					<Controls action=""disabling"">
						<Control tag=""21""/>
						<Control tag=""23""/>
					</Controls>
				</Select>
				<Select case=""true"">
					<Controls action=""enabling"">
						<Control tag=""21""/>
						<Control tag=""23""/>
					</Controls>
					<Controls action=""disabling"">
						<Control tag=""0""/>
						<Control tag=""1""/>
						<Control tag=""2""/>
						<Control tag=""3""/>
						<Control tag=""4""/>
						<Control tag=""5""/>
						<Control tag=""6""/>
						<Control tag=""7""/>
						<Control tag=""8""/>
						<Control tag=""9""/>
						<Control tag=""11""/>
						<Control tag=""13""/>						
						<Control tag=""25""/>
						<Control tag=""26""/>
					</Controls>
				</Select>
			</Control>
			<Control tag=""17""/>
			<Control tag=""21""/>
			<Control tag=""23""/>
		</Controls>
		<Controls action=""disabling"">
			<Control tag=""0""/>
			<Control tag=""1""/>
			<Control tag=""2""/>
			<Control tag=""3""/>
			<Control tag=""4""/>
			<Control tag=""5""/>
			<Control tag=""6""/>
			<Control tag=""7""/>
			<Control tag=""8""/>
			<Control tag=""9""/>
			<Control tag=""11""/>
			<Control tag=""13""/>
			<Control tag=""16""/>
			<Control tag=""19""/>
			<Control tag=""25""/>
			<Control tag=""26""/>
		</Controls>
	</Type>
	<Type name=""datetime"">
		<Controls action=""enabling"">
			<Control tag=""17""/>
			<Control tag=""21""/>
			<Control tag=""23""/>
		</Controls>
		<Controls action=""disabling"">
			<Control tag=""0""/>
			<Control tag=""2""/>
			<Control tag=""3""/>
			<Control tag=""4""/>
			<Control tag=""5""/>
			<Control tag=""6""/>
			<Control tag=""7""/>
			<Control tag=""9""/>
			<Control tag=""11""/>
			<Control tag=""13""/>
			<Control tag=""15""/>
			<Control tag=""16""/>
			<Control tag=""19""/>
			<Control tag=""25""/>
			<Control tag=""26""/>
		</Controls>
	</Type>
	<Type name=""boolean"">
		<Controls action=""enabling"">
			<Control tag=""0"">
				<Select case=""false"">
					<Controls action=""disabling"">
						<Control tag=""1""/> <!-- Schema Combo -->
						<Control tag=""2""/> <!-- High Access Control -->
						<Control tag=""3""/> <!-- Table/Domain -->
						<Control tag=""4""/> <!-- Column Code -->
						<Control tag=""5""/> <!-- Column Value -->
						<Control tag=""6""/> <!-- Show Meaning -->
						<Control tag=""7""/> <!-- Select Option -->
						<Control tag=""8""/> <!-- Distinct -->
						<Control tag=""9""/> <!-- Default Values -->
						<Control tag=""11""/> <!-- RowNum -->
						<Control tag=""13""/> <!-- Order By -->
						<Control tag=""25""/> <!-- IS NOT NULLS -->
						<Control tag=""26""/> <!-- Where -->
					</Controls>
					<Controls action=""enabling""/>
				</Select>
				<Select case=""true"">
					<Controls action=""enabling"">
						<Control tag=""1""/> <!-- Schema Combo -->
						<Control tag=""2""/> <!-- High Access Control -->
						<Control tag=""3""/> <!-- Table/Domain -->
						<Control tag=""4""/> <!-- Column Code -->
						<Control tag=""5""/> <!-- Column Value -->
						<Control tag=""6""/> <!-- Show Meaning -->
						<Control tag=""7""/> <!-- Select Option -->
						<Control tag=""8""/> <!-- Distinct -->
						<Control tag=""9""/> <!-- Default Values -->
						<Control tag=""11""/> <!-- RowNum -->
						<Control tag=""13""/> <!-- Order By -->
						<Control tag=""25""/> <!-- IS NOT NULLS -->
						<Control tag=""26""/> <!-- Where -->
					</Controls>
					<Controls action=""disabling""/>
				</Select>
			</Control>
			<Control tag=""2"">
				<Select case=""0"">
					<Controls action=""enabling"">
						<Control tag=""7""/>
						<Control tag=""8""/>
						<Control tag=""9""/>
						<Control tag=""11""/>
						<Control tag=""13""/>
						<Control tag=""25""/>						
						<Control tag=""26""/>
					</Controls>
					<Controls action=""disabling""/>						
				</Select>
				<Select case=""1"">
					<Controls action=""enabling"">
						<Control tag=""7""/>
						<Control tag=""8""/>
						<Control tag=""9""/>
						<Control tag=""11""/>
						<Control tag=""13""/>
						<Control tag=""25""/>
						<Control tag=""26""/>
					</Controls>
					<Controls action=""disabling""/>
				</Select>
				<Select case=""2"">
					<Controls action=""enabling"">
						<Control tag=""7""/>
						<Control tag=""8""/>
						<Control tag=""9""/>
						<Control tag=""11""/>
						<Control tag=""13""/>
						<Control tag=""25""/>
						<Control tag=""26""/>
					</Controls>
					<Controls action=""disabling""/>
				</Select>
			</Control>
			<Control tag=""3""/>
			<Control tag=""4""/>
			<Control tag=""5""/>
			<Control tag=""6""/>
			<Control tag=""7""/>
			<Control tag=""8""/>
			<Control tag=""9""/>
			<Control tag=""11""/>
			<Control tag=""13""/>
			<Control tag=""25""/>
			<Control tag=""26""/>
		</Controls>
		<Controls action=""disabling"">
			<Control tag=""15""/>
			<Control tag=""16""/>
			<Control tag=""17""/>
			<Control tag=""19""/>
			<Control tag=""21""/>
			<Control tag=""23""/>
		</Controls>
	</Type>
	<ShowHide>
		<Control tag=""9"">
			<Control tag=""10""/>
		</Control>
		<Control tag=""11"">
			<Control tag=""12""/>
		</Control>
		<Control tag=""13"">
			<Control tag=""14""/>
		</Control>
		<Control tag=""17"">
			<Control tag=""18""/>
		</Control>
		<Control tag=""19"">
			<Control tag=""20""/>
		</Control>
		<Control tag=""21"">
			<Control tag=""22""/>
		</Control>
		<Control tag=""23"">
			<Control tag=""24""/>
		</Control>
		<Control tag=""25"">
			<Control tag=""28""/>
		</Control>		
		<Control tag=""26"">
			<Control tag=""27""/>
		</Control>
	</ShowHide>
</FilterTemplate>");
      #endregion
   }
}
