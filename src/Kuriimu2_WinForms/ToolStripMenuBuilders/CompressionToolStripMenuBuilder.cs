﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Kontract.Attributes;
using Kontract.Attributes.Intermediate;
using Kontract.Interfaces.Intermediate;
using Kuriimu2_WinForms.Extensions;

namespace Kuriimu2_WinForms.ToolStripMenuBuilders
{
    // TODO: Combine ToolStripBuilders
    class CompressionToolStripMenuBuilder
    {
        private readonly List<ToolStripMenuItem> _tree;
        private readonly Action<ToolStripMenuItem, ICompressionAdapter, bool, bool> _addItemDelegates;

        public CompressionToolStripMenuBuilder(IEnumerable<ICompressionAdapter> adapters, Action<ToolStripMenuItem, ICompressionAdapter, bool, bool> addItemDelegates)
        {
            _addItemDelegates = addItemDelegates;
            _tree = CreateTree(adapters).ToList();
        }

        public void AddTreeToMenuStrip(ToolStripMenuItem adapterItem)
        {
            foreach (var node in _tree)
                adapterItem.DropDownItems.Add(node);
        }

        public IEnumerable<ToolStripMenuItem> CreateTree(IEnumerable<ICompressionAdapter> adapters)
        {
            var result = new List<ToolStripMenuItem>();

            foreach (var adapter in adapters)
            {
                var attr = adapter.GetType().GetCustomAttributes(typeof(MenuStripExtensionAttribute), false).Cast<MenuStripExtensionAttribute>().FirstOrDefault();

                var ignoreComp = adapter.GetType().GetCustomAttributes(typeof(IgnoreCompressionAttribute), false).Any();
                var ignoreDec = adapter.GetType().GetCustomAttributes(typeof(IgnoreDecompressionAttribute), false).Any();

                if (ignoreDec && ignoreComp)
                    continue;

                if (attr == null)
                {
                    var nodeItem = result.FirstOrDefault(x => x.Name == "Others");
                    if (nodeItem == null)
                    {
                        nodeItem = new ToolStripMenuItem("Others");
                        result.Add(nodeItem);
                    }

                    var cipherItem = new ToolStripMenuItem(adapter.Name);
                    _addItemDelegates(cipherItem, adapter, ignoreDec, ignoreComp);

                    nodeItem.DropDownItems.Add(cipherItem);
                }
                else
                {
                    ToolStripItemCollection items = null;
                    ToolStripMenuItem internalItem = null;

                    foreach (var strip in attr.Strips)
                    {
                        if (items == null)
                            internalItem = result.FirstOrDefault(x => x.Name == strip);
                        else
                        {
                            var index = items.IndexByName(strip);
                            if (index < 0)
                                internalItem = null;
                            else
                                internalItem = items[index] as ToolStripMenuItem;
                        }

                        if (internalItem == null)
                        {
                            internalItem = new ToolStripMenuItem(strip) { Name = strip };
                            if (items == null)
                                result.Add(internalItem);
                            else
                                items.Add(internalItem);
                        }

                        items = internalItem.DropDownItems;
                    }

                    _addItemDelegates(internalItem, adapter, ignoreDec, ignoreComp);
                }
            }

            return result;
        }
    }
}
