﻿namespace Kuriimu2_WinForms.MainForms
{
    partial class Kuriimu2
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.mnuMain = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openWithPluginToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textSequenceSearcherToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.batchProcessorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ciphersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hashesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compressionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rawImageViewerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageTranscoderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFiles = new System.Windows.Forms.TabControl();
            this.tabCloseButtons = new System.Windows.Forms.ImageList(this.components);
            this.pnlMain = new System.Windows.Forms.Panel();
            this.operationStatusBar = new System.Windows.Forms.StatusStrip();
            this.operationProgress = new Kuriimu2_WinForms.Controls.InfoToolStripProgressBar();
            this.operationTimer = new System.Windows.Forms.ToolStripStatusLabel();
            this.mnuMain.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.operationStatusBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // mnuMain
            // 
            this.mnuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.ciphersToolStripMenuItem,
            this.hashesToolStripMenuItem,
            this.compressionsToolStripMenuItem,
            this.rawImageViewerToolStripMenuItem,
            this.imageTranscoderToolStripMenuItem});
            this.mnuMain.Location = new System.Drawing.Point(0, 0);
            this.mnuMain.Name = "mnuMain";
            this.mnuMain.Padding = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.mnuMain.Size = new System.Drawing.Size(957, 24);
            this.mnuMain.TabIndex = 0;
            this.mnuMain.Text = "mainMenuStrip";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.openWithPluginToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.openToolStripMenuItem.Text = "&Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // openWithPluginToolStripMenuItem
            // 
            this.openWithPluginToolStripMenuItem.Name = "openWithPluginToolStripMenuItem";
            this.openWithPluginToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.openWithPluginToolStripMenuItem.Text = "Open with &Plugin";
            this.openWithPluginToolStripMenuItem.Click += new System.EventHandler(this.openWithPluginToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.textSequenceSearcherToolStripMenuItem,
            this.batchProcessorToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // textSequenceSearcherToolStripMenuItem
            // 
            this.textSequenceSearcherToolStripMenuItem.Name = "textSequenceSearcherToolStripMenuItem";
            this.textSequenceSearcherToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.textSequenceSearcherToolStripMenuItem.Text = "Text Sequence Searcher";
            this.textSequenceSearcherToolStripMenuItem.Click += new System.EventHandler(this.TextSequenceSearcherToolStripMenuItem_Click);
            // 
            // batchProcessorToolStripMenuItem
            // 
            this.batchProcessorToolStripMenuItem.Name = "batchProcessorToolStripMenuItem";
            this.batchProcessorToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.batchProcessorToolStripMenuItem.Text = "Batch Processor";
            this.batchProcessorToolStripMenuItem.Click += new System.EventHandler(this.BatchProcessorToolStripMenuItem_Click);
            // 
            // ciphersToolStripMenuItem
            // 
            this.ciphersToolStripMenuItem.Enabled = false;
            this.ciphersToolStripMenuItem.Name = "ciphersToolStripMenuItem";
            this.ciphersToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.ciphersToolStripMenuItem.Text = "Ciphers";
            // 
            // hashesToolStripMenuItem
            // 
            this.hashesToolStripMenuItem.Enabled = false;
            this.hashesToolStripMenuItem.Name = "hashesToolStripMenuItem";
            this.hashesToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.hashesToolStripMenuItem.Text = "Hashes";
            // 
            // compressionsToolStripMenuItem
            // 
            this.compressionsToolStripMenuItem.Enabled = false;
            this.compressionsToolStripMenuItem.Name = "compressionsToolStripMenuItem";
            this.compressionsToolStripMenuItem.Size = new System.Drawing.Size(94, 20);
            this.compressionsToolStripMenuItem.Text = "Compressions";
            // 
            // rawImageViewerToolStripMenuItem
            // 
            this.rawImageViewerToolStripMenuItem.Enabled = false;
            this.rawImageViewerToolStripMenuItem.Name = "rawImageViewerToolStripMenuItem";
            this.rawImageViewerToolStripMenuItem.Size = new System.Drawing.Size(115, 20);
            this.rawImageViewerToolStripMenuItem.Text = "Raw Image Viewer";
            this.rawImageViewerToolStripMenuItem.Click += new System.EventHandler(this._imgDecToolStrip_Click);
            // 
            // imageTranscoderToolStripMenuItem
            // 
            this.imageTranscoderToolStripMenuItem.Enabled = false;
            this.imageTranscoderToolStripMenuItem.Name = "imageTranscoderToolStripMenuItem";
            this.imageTranscoderToolStripMenuItem.Size = new System.Drawing.Size(113, 20);
            this.imageTranscoderToolStripMenuItem.Text = "Image Transcoder";
            this.imageTranscoderToolStripMenuItem.Click += new System.EventHandler(this._imgTransToolStrip_Click);
            // 
            // openFiles
            // 
            this.openFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.openFiles.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.openFiles.ImageList = this.tabCloseButtons;
            this.openFiles.Location = new System.Drawing.Point(3, 2);
            this.openFiles.Margin = new System.Windows.Forms.Padding(0);
            this.openFiles.Name = "openFiles";
            this.openFiles.Padding = new System.Drawing.Point(8, 3);
            this.openFiles.SelectedIndex = 0;
            this.openFiles.Size = new System.Drawing.Size(953, 507);
            this.openFiles.TabIndex = 1;
            this.openFiles.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.openFiles_DrawItem);
            this.openFiles.MouseUp += new System.Windows.Forms.MouseEventHandler(this.openFiles_MouseUp);
            // 
            // tabCloseButtons
            // 
            this.tabCloseButtons.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.tabCloseButtons.ImageSize = new System.Drawing.Size(16, 16);
            this.tabCloseButtons.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.openFiles);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 24);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Padding = new System.Windows.Forms.Padding(3, 2, 1, 2);
            this.pnlMain.Size = new System.Drawing.Size(957, 511);
            this.pnlMain.TabIndex = 2;
            // 
            // operationStatusBar
            // 
            this.operationStatusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.operationProgress,
            this.operationTimer});
            this.operationStatusBar.Location = new System.Drawing.Point(0, 535);
            this.operationStatusBar.Name = "operationStatusBar";
            this.operationStatusBar.Size = new System.Drawing.Size(957, 22);
            this.operationStatusBar.TabIndex = 3;
            // 
            // operationProgress
            // 
            this.operationProgress.Name = "operationProgress";
            this.operationProgress.ProgressColor = System.Drawing.Color.ForestGreen;
            this.operationProgress.Size = new System.Drawing.Size(500, 16);
            this.operationProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.operationProgress.TextColor = System.Drawing.Color.Black;
            // 
            // operationTimer
            // 
            this.operationTimer.Name = "operationTimer";
            this.operationTimer.Size = new System.Drawing.Size(0, 17);
            // 
            // Kuriimu2
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(957, 557);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.operationStatusBar);
            this.Controls.Add(this.mnuMain);
            this.MainMenuStrip = this.mnuMain;
            this.Name = "Kuriimu2";
            this.Text = "Kuriimu2";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Kuriimu2_FormClosing);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Kuriimu2_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Kuriimu2_DragEnter);
            this.mnuMain.ResumeLayout(false);
            this.mnuMain.PerformLayout();
            this.pnlMain.ResumeLayout(false);
            this.operationStatusBar.ResumeLayout(false);
            this.operationStatusBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mnuMain;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.TabControl openFiles;
        private System.Windows.Forms.ImageList tabCloseButtons;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.StatusStrip operationStatusBar;
        private Controls.InfoToolStripProgressBar operationProgress;
        private System.Windows.Forms.ToolStripStatusLabel operationTimer;
        private System.Windows.Forms.ToolStripMenuItem openWithPluginToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem textSequenceSearcherToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem batchProcessorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ciphersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hashesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem compressionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rawImageViewerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem imageTranscoderToolStripMenuItem;
    }
}

