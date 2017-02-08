namespace DIAWebServerExample
{
    partial class Form_CariKartListesi
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lbl_Firma = new System.Windows.Forms.Label();
            this.lbl_Donem = new System.Windows.Forms.Label();
            this.cmb_Firma = new System.Windows.Forms.ComboBox();
            this.cmb_Donem = new System.Windows.Forms.ComboBox();
            this.btn_Getir = new System.Windows.Forms.Button();
            this.btn_Ekle = new System.Windows.Forms.Button();
            this.btn_Degistir = new System.Windows.Forms.Button();
            this.btn_Sil = new System.Windows.Forms.Button();
            this.btn_Kapat = new System.Windows.Forms.Button();
            this.scfuretimfisisilresp1BindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.gbx_CariListe = new System.Windows.Forms.DataGridView();
            this.Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bakiyeSorgula = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.scfuretimfisisilresp1BindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbx_CariListe)).BeginInit();
            this.SuspendLayout();
            // 
            // lbl_Firma
            // 
            this.lbl_Firma.AutoSize = true;
            this.lbl_Firma.Location = new System.Drawing.Point(12, 12);
            this.lbl_Firma.Name = "lbl_Firma";
            this.lbl_Firma.Size = new System.Drawing.Size(62, 13);
            this.lbl_Firma.TabIndex = 0;
            this.lbl_Firma.Text = "Firma Seçin";
            // 
            // lbl_Donem
            // 
            this.lbl_Donem.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lbl_Donem.AutoSize = true;
            this.lbl_Donem.Location = new System.Drawing.Point(247, 15);
            this.lbl_Donem.Name = "lbl_Donem";
            this.lbl_Donem.Size = new System.Drawing.Size(71, 13);
            this.lbl_Donem.TabIndex = 1;
            this.lbl_Donem.Text = "Dönem Seçin";
            // 
            // cmb_Firma
            // 
            this.cmb_Firma.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_Firma.FormattingEnabled = true;
            this.cmb_Firma.Location = new System.Drawing.Point(80, 12);
            this.cmb_Firma.Name = "cmb_Firma";
            this.cmb_Firma.Size = new System.Drawing.Size(161, 21);
            this.cmb_Firma.TabIndex = 2;
            this.cmb_Firma.SelectedIndexChanged += new System.EventHandler(this.cmb_Firma_SelectedIndexChanged);
            // 
            // cmb_Donem
            // 
            this.cmb_Donem.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.cmb_Donem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_Donem.FormattingEnabled = true;
            this.cmb_Donem.Location = new System.Drawing.Point(324, 12);
            this.cmb_Donem.Name = "cmb_Donem";
            this.cmb_Donem.Size = new System.Drawing.Size(187, 21);
            this.cmb_Donem.TabIndex = 3;
            // 
            // btn_Getir
            // 
            this.btn_Getir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Getir.AutoSize = true;
            this.btn_Getir.Location = new System.Drawing.Point(555, 12);
            this.btn_Getir.Name = "btn_Getir";
            this.btn_Getir.Size = new System.Drawing.Size(85, 23);
            this.btn_Getir.TabIndex = 4;
            this.btn_Getir.Text = "Getir";
            this.btn_Getir.UseVisualStyleBackColor = true;
            this.btn_Getir.Click += new System.EventHandler(this.btn_Getir_Click);
            // 
            // btn_Ekle
            // 
            this.btn_Ekle.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btn_Ekle.Location = new System.Drawing.Point(15, 441);
            this.btn_Ekle.Name = "btn_Ekle";
            this.btn_Ekle.Size = new System.Drawing.Size(75, 23);
            this.btn_Ekle.TabIndex = 5;
            this.btn_Ekle.Text = "Ekle";
            this.btn_Ekle.UseVisualStyleBackColor = true;
            this.btn_Ekle.Click += new System.EventHandler(this.btn_Ekle_Click);
            // 
            // btn_Degistir
            // 
            this.btn_Degistir.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btn_Degistir.Location = new System.Drawing.Point(113, 441);
            this.btn_Degistir.Name = "btn_Degistir";
            this.btn_Degistir.Size = new System.Drawing.Size(75, 23);
            this.btn_Degistir.TabIndex = 6;
            this.btn_Degistir.Text = "Değiştir";
            this.btn_Degistir.UseVisualStyleBackColor = true;
            this.btn_Degistir.Click += new System.EventHandler(this.btn_Degistir_Click);
            // 
            // btn_Sil
            // 
            this.btn_Sil.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btn_Sil.Location = new System.Drawing.Point(224, 441);
            this.btn_Sil.Name = "btn_Sil";
            this.btn_Sil.Size = new System.Drawing.Size(75, 23);
            this.btn_Sil.TabIndex = 7;
            this.btn_Sil.Text = "Sil";
            this.btn_Sil.UseVisualStyleBackColor = true;
            this.btn_Sil.Click += new System.EventHandler(this.btn_Sil_Click);
            // 
            // btn_Kapat
            // 
            this.btn_Kapat.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btn_Kapat.Location = new System.Drawing.Point(565, 441);
            this.btn_Kapat.Name = "btn_Kapat";
            this.btn_Kapat.Size = new System.Drawing.Size(75, 23);
            this.btn_Kapat.TabIndex = 8;
            this.btn_Kapat.Text = "Kapat";
            this.btn_Kapat.UseVisualStyleBackColor = true;
            this.btn_Kapat.Click += new System.EventHandler(this.btn_Kapat_Click);
            // 
            // gbx_CariListe
            // 
            this.gbx_CariListe.AllowUserToAddRows = false;
            this.gbx_CariListe.AllowUserToDeleteRows = false;
            this.gbx_CariListe.AllowUserToOrderColumns = true;
            this.gbx_CariListe.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbx_CariListe.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gbx_CariListe.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column9,
            this.Column10,
            this.Column11});
            this.gbx_CariListe.Location = new System.Drawing.Point(13, 61);
            this.gbx_CariListe.MultiSelect = false;
            this.gbx_CariListe.Name = "gbx_CariListe";
            this.gbx_CariListe.ReadOnly = true;
            this.gbx_CariListe.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gbx_CariListe.Size = new System.Drawing.Size(627, 374);
            this.gbx_CariListe.TabIndex = 9;
            // 
            // Column9
            // 
            this.Column9.HeaderText = "Column9";
            this.Column9.Name = "Column9";
            this.Column9.ReadOnly = true;
            // 
            // Column10
            // 
            this.Column10.HeaderText = "Column10";
            this.Column10.Name = "Column10";
            this.Column10.ReadOnly = true;
            this.Column10.Width = 150;
            // 
            // Column11
            // 
            this.Column11.HeaderText = "Column11";
            this.Column11.Name = "Column11";
            this.Column11.ReadOnly = true;
            // 
            // bakiyeSorgula
            // 
            this.bakiyeSorgula.Location = new System.Drawing.Point(461, 441);
            this.bakiyeSorgula.Name = "bakiyeSorgula";
            this.bakiyeSorgula.Size = new System.Drawing.Size(75, 23);
            this.bakiyeSorgula.TabIndex = 10;
            this.bakiyeSorgula.Text = "Bakiye Sorgula";
            this.bakiyeSorgula.UseVisualStyleBackColor = true;
            this.bakiyeSorgula.Click += new System.EventHandler(this.bakiyeSorgula_Click);
            // 
            // Form_CariKartListesi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(652, 502);
            this.Controls.Add(this.bakiyeSorgula);
            this.Controls.Add(this.gbx_CariListe);
            this.Controls.Add(this.btn_Kapat);
            this.Controls.Add(this.btn_Sil);
            this.Controls.Add(this.btn_Degistir);
            this.Controls.Add(this.btn_Ekle);
            this.Controls.Add(this.btn_Getir);
            this.Controls.Add(this.cmb_Donem);
            this.Controls.Add(this.cmb_Firma);
            this.Controls.Add(this.lbl_Donem);
            this.Controls.Add(this.lbl_Firma);
            this.MinimumSize = new System.Drawing.Size(600, 450);
            this.Name = "Form_CariKartListesi";
            this.Text = "Cari Kart Listesi";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form_CariKartListesi_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.scfuretimfisisilresp1BindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbx_CariListe)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_Firma;
        private System.Windows.Forms.Label lbl_Donem;
        private System.Windows.Forms.ComboBox cmb_Firma;
        private System.Windows.Forms.ComboBox cmb_Donem;
        private System.Windows.Forms.Button btn_Getir;
        private System.Windows.Forms.Button btn_Ekle;
        private System.Windows.Forms.Button btn_Degistir;
        private System.Windows.Forms.Button btn_Sil;
        private System.Windows.Forms.Button btn_Kapat;
        private System.Windows.Forms.BindingSource scfuretimfisisilresp1BindingSource;
        private System.Windows.Forms.DataGridView gbx_CariListe;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column9;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column10;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column11;
        private System.Windows.Forms.Button bakiyeSorgula;
    }
}