namespace System.Supplies.Ui.MasterPage
{
   partial class MAIN_PAGE_F
   {
      /// <summary> 
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      /// <summary> 
      /// Clean up any resources being used.
      /// </summary>
      /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
      protected override void Dispose(bool disposing)
      {
         if (disposing && (components != null))
         {
            components.Dispose();
         }
         base.Dispose(disposing);
      }

      #region Component Designer generated code

      /// <summary> 
      /// Required method for Designer support - do not modify 
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MAIN_PAGE_F));
         this.ribbonControl1 = new DevExpress.XtraBars.Ribbon.RibbonControl();
         this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
         this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
         this.bbi_hospbutn = new DevExpress.XtraBars.BarButtonItem();
         this.bbi_paygbutn = new DevExpress.XtraBars.BarButtonItem();
         this.bbi_compbutn = new DevExpress.XtraBars.BarButtonItem();
         this.ribbonPage2 = new DevExpress.XtraBars.Ribbon.RibbonPage();
         this.ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
         this.bbi_fatcbutn = new DevExpress.XtraBars.BarButtonItem();
         this.bbi_fctlbutn = new DevExpress.XtraBars.BarButtonItem();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
         this.SuspendLayout();
         // 
         // ribbonControl1
         // 
         this.ribbonControl1.ExpandCollapseItem.Id = 0;
         this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl1.ExpandCollapseItem,
            this.bbi_hospbutn,
            this.bbi_paygbutn,
            this.bbi_compbutn,
            this.bbi_fatcbutn,
            this.bbi_fctlbutn});
         this.ribbonControl1.Location = new System.Drawing.Point(0, 0);
         this.ribbonControl1.MaxItemId = 6;
         this.ribbonControl1.Name = "ribbonControl1";
         this.ribbonControl1.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPage1,
            this.ribbonPage2});
         this.ribbonControl1.Size = new System.Drawing.Size(889, 142);
         // 
         // ribbonPage1
         // 
         this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup1});
         this.ribbonPage1.Name = "ribbonPage1";
         this.ribbonPage1.Text = "اطلاعات پایه";
         // 
         // ribbonPageGroup1
         // 
         this.ribbonPageGroup1.ItemLinks.Add(this.bbi_hospbutn);
         this.ribbonPageGroup1.ItemLinks.Add(this.bbi_paygbutn);
         this.ribbonPageGroup1.ItemLinks.Add(this.bbi_compbutn);
         this.ribbonPageGroup1.Name = "ribbonPageGroup1";
         this.ribbonPageGroup1.Text = "تعاریف";
         // 
         // bbi_hospbutn
         // 
         this.bbi_hospbutn.Caption = "بیمارستان";
         this.bbi_hospbutn.CategoryGuid = new System.Guid("6ffddb2b-9015-4d97-a4c1-91613e0ef537");
         this.bbi_hospbutn.Glyph = ((System.Drawing.Image)(resources.GetObject("bbi_hospbutn.Glyph")));
         this.bbi_hospbutn.Id = 1;
         this.bbi_hospbutn.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("bbi_hospbutn.LargeGlyph")));
         this.bbi_hospbutn.Name = "bbi_hospbutn";
         // 
         // bbi_paygbutn
         // 
         this.bbi_paygbutn.Caption = "سرفصل";
         this.bbi_paygbutn.CategoryGuid = new System.Guid("6ffddb2b-9015-4d97-a4c1-91613e0ef537");
         this.bbi_paygbutn.Glyph = ((System.Drawing.Image)(resources.GetObject("bbi_paygbutn.Glyph")));
         this.bbi_paygbutn.Id = 2;
         this.bbi_paygbutn.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("bbi_paygbutn.LargeGlyph")));
         this.bbi_paygbutn.Name = "bbi_paygbutn";
         // 
         // bbi_compbutn
         // 
         this.bbi_compbutn.Caption = "شرکت";
         this.bbi_compbutn.CategoryGuid = new System.Guid("6ffddb2b-9015-4d97-a4c1-91613e0ef537");
         this.bbi_compbutn.Glyph = ((System.Drawing.Image)(resources.GetObject("bbi_compbutn.Glyph")));
         this.bbi_compbutn.Id = 3;
         this.bbi_compbutn.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("bbi_compbutn.LargeGlyph")));
         this.bbi_compbutn.Name = "bbi_compbutn";
         // 
         // ribbonPage2
         // 
         this.ribbonPage2.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup2});
         this.ribbonPage2.Name = "ribbonPage2";
         this.ribbonPage2.Text = "اطلاعات و ثبت ها";
         // 
         // ribbonPageGroup2
         // 
         this.ribbonPageGroup2.ItemLinks.Add(this.bbi_fatcbutn);
         this.ribbonPageGroup2.ItemLinks.Add(this.bbi_fctlbutn);
         this.ribbonPageGroup2.Name = "ribbonPageGroup2";
         this.ribbonPageGroup2.Text = "فاکتور";
         // 
         // bbi_fatcbutn
         // 
         this.bbi_fatcbutn.Caption = "ثبت فاکتور";
         this.bbi_fatcbutn.CategoryGuid = new System.Guid("6ffddb2b-9015-4d97-a4c1-91613e0ef537");
         this.bbi_fatcbutn.Glyph = ((System.Drawing.Image)(resources.GetObject("bbi_fatcbutn.Glyph")));
         this.bbi_fatcbutn.Id = 4;
         this.bbi_fatcbutn.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("bbi_fatcbutn.LargeGlyph")));
         this.bbi_fatcbutn.Name = "bbi_fatcbutn";
         // 
         // bbi_fctlbutn
         // 
         this.bbi_fctlbutn.Caption = "لیست فاکتور";
         this.bbi_fctlbutn.CategoryGuid = new System.Guid("6ffddb2b-9015-4d97-a4c1-91613e0ef537");
         this.bbi_fctlbutn.Glyph = ((System.Drawing.Image)(resources.GetObject("bbi_fctlbutn.Glyph")));
         this.bbi_fctlbutn.Id = 5;
         this.bbi_fctlbutn.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("bbi_fctlbutn.LargeGlyph")));
         this.bbi_fctlbutn.Name = "bbi_fctlbutn";
         // 
         // MAIN_PAGE_F
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.ribbonControl1);
         this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.Name = "MAIN_PAGE_F";
         this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
         this.Size = new System.Drawing.Size(889, 535);
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
      private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
      private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
      private DevExpress.XtraBars.BarButtonItem bbi_hospbutn;
      private DevExpress.XtraBars.BarButtonItem bbi_paygbutn;
      private DevExpress.XtraBars.BarButtonItem bbi_compbutn;
      private DevExpress.XtraBars.BarButtonItem bbi_fatcbutn;
      private DevExpress.XtraBars.BarButtonItem bbi_fctlbutn;
      private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage2;
      private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
   }
}
