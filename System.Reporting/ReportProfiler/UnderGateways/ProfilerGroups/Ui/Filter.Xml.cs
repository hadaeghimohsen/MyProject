using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace System.Reporting.ReportProfiler.UnderGateways.ProfilerGroups.Ui
{
    partial class Filter
    {
        string xmlinfo = @"
<Filtering>
	<!-- Direct input -->
	<SourcePath type=""*"">
		<Field type=""int"">
			<Comparing>
				<Item type=""equal"" fa=""مساوی"" en="""" control=""text"" count=""1""/>
				<Item type=""not equal"" fa=""نامساوی"" en="""" control=""text"" count=""1""/>
				<Item type=""less than"" fa=""بزرگتر از"" en="""" control=""text"" count=""1""/>
				<Item type=""less than or equal"" fa=""بزرگتر مساوی از"" en="""" control=""text"" count=""1""/>
				<Item type=""greater"" fa=""کوچکتر از"" en="""" control=""text"" count=""1""/>
				<Item type=""greater than or equal"" fa=""کوچکتر مساوی از"" en="""" control=""text"" count=""1""/>
				<Item type=""between"" fa=""بین دو بازه"" en="""" control=""text"" count=""2""/>
				<Item type=""not between"" fa=""غیراز بازه"" en="""" control=""text"" count=""2""/>
				<Item type=""is null"" fa=""مقادیر پوچ"" en="""" control="""" count=""""/>
				<Item type=""is not null"" fa=""غیر از مقادیر پوچ"" en="""" control="""" count=""""/>				
			</Comparing>
		</Field>
		<Field type=""string"">
			<Comparing>
				<Item type=""equal"" fa=""مساوی"" en="""" control=""text"" count=""1""/>
				<Item type=""not equal"" fa=""نامساوی"" en="""" control=""text"" count=""1""/>
				<Item type=""less than"" fa=""بزرگتر از"" en="""" control=""text"" count=""1""/>
				<Item type=""less than or equal"" fa=""بزرگتر مساوی از"" en="""" control=""text"" count=""1""/>
				<Item type=""greater than"" fa=""کوچکتر از"" en="""" control=""text"" count=""1""/>
				<Item type=""greater than or equal"" fa=""کوچکتر مساوی از"" en="""" control=""text"" count=""1""/>
				<Item type=""startwith"" fa=""شروع شود"" en="""" control=""text"" count=""1""/>
				<Item type=""not startwith"" fa=""شروع نشود"" en="""" control=""text"" count=""1""/>
				<Item type=""endwith"" fa=""پایان یابد"" en="""" control=""text"" count=""1""/>
				<Item type=""not endtwith"" fa=""پایان نیابد"" en="""" control=""text"" count=""1""/>
				<Item type=""like"" fa=""شامل باشد"" en="""" control=""text"" count=""1""/>
				<Item type=""not like"" fa=""شامل نباشد"" en="""" control=""text"" count=""1""/>
				<Item type=""between"" fa=""بین دو بازه"" en="""" control=""text"" count=""2""/>
				<Item type=""not between"" fa=""غیراز بازه"" en="""" control=""text"" count=""2""/>
				<Item type=""is null"" fa=""مقادیر پوچ"" en="""" control="""" count=""""/>
				<Item type=""is not null"" fa=""غیر از مقادیر پوچ"" en="""" control="""" count=""""/>				
			</Comparing>
		</Field>
		<Field type=""datetime"">
			<Comparing>
				<Item type=""equal"" fa=""مساوی"" en="""" control=""text"" count=""1""/>
				<Item type=""not equal"" fa=""نامساوی"" en="""" control=""text"" count=""1""/>
				<Item type=""less than"" fa=""بزرگتر از"" en="""" control=""text"" count=""1""/>
				<Item type=""less than or equal"" fa=""بزرگتر مساوی از"" en="""" control=""text"" count=""1""/>
				<Item type=""greater than"" fa=""کوچکتر از"" en="""" control=""text"" count=""1""/>
				<Item type=""greater than or equal"" fa=""کوچکتر مساوی از"" en="""" control=""text"" count=""1""/>
				<Item type=""between"" fa=""بین دو بازه"" en="""" control=""text"" count=""2""/>
				<Item type=""not between"" fa=""غیراز بازه"" en="""" control=""text"" count=""2""/>
				<Item type=""is null"" fa=""مقادیر پوچ"" en="""" control="""" count=""""/>
				<Item type=""is not null"" fa=""غیر از مقادیر پوچ"" en="""" control="""" count=""""/>				
			</Comparing>
		</Field>
      <Field type=""domain"">
			<Comparing>
				<Item type=""is null"" fa=""مقادیر پوچ"" en="""" control="""" count=""""/>
				<Item type=""is not null"" fa=""غیر از مقادیر پوچ"" en="""" control="""" count=""""/>				
			</Comparing>
		</Field>
	</SourcePath>
	<!-- From file -->
	<SourcePath type=""#"">
		<Field type=""int"">
			<Comparing>
				<Item type=""in"" fa=""شامل مقادیرمشخص شده"" en="""" control=""combobox"" count=""1""/>
				<Item type=""not in"" fa=""غیر از مقادیر مشخص شده"" en="""" control=""combobox"" count=""1""/>
			</Comparing>
		</Field>
		<Field type=""string"">
			<Comparing>
				<Item type=""in"" fa=""شامل مقادیرمشخص شده"" en="""" control=""combobox"" count=""1""/>
				<Item type=""not in"" fa=""غیر از مقادیر مشخص شده"" en="""" control=""combobox"" count=""1""/>
			</Comparing>
		</Field>
		<Field type=""datetime"">
			<Comparing>
				<Item type=""in"" fa=""شامل مقادیرمشخص شده"" en="""" control=""combobox"" count=""1""/>
				<Item type=""not in"" fa=""غیر از مقادیر مشخص شده"" en="""" control=""combobox"" count=""1""/>
			</Comparing>
		</Field>
      <Field type=""domain"">
			<Comparing>
				
			</Comparing>
		</Field>
	</SourcePath>
	<!-- From datasource -->
	<SourcePath type=""@"">
		<Field type=""int"">
			<Comparing>
				<Item type=""in"" fa=""شامل مقادیرمشخص شده"" en="""" control=""combobox"" count=""1""/>
				<Item type=""not in"" fa=""غیر از مقادیر مشخص شده"" en="""" control=""combobox"" count=""1""/>
			</Comparing>
		</Field>
		<Field type=""string"">
			<Comparing>
				<Item type=""in"" fa=""شامل مقادیرمشخص شده"" en="""" control=""combobox"" count=""1""/>
				<Item type=""not in"" fa=""غیر از مقادیر مشخص شده"" en="""" control=""combobox"" count=""1""/>
			</Comparing>
		</Field>
      <Field type=""domain"">
			<Comparing>
				<Item type=""in"" fa=""شامل مقادیرمشخص شده"" en="""" control=""combobox"" count=""1""/>
				<Item type=""not in"" fa=""غیر از مقادیر مشخص شده"" en="""" control=""combobox"" count=""1""/>
			</Comparing>
		</Field>
	</SourcePath>
</Filtering>";

        private XDocument xmlUi;
        private XDocument XmlUi
        {
           get
           {
              if (xmlUi == null)
                 xmlUi = XDocument.Parse(xmlinfo);
              return xmlUi;
           }
        }

    }
}
