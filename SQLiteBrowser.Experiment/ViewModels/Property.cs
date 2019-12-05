﻿using System;

namespace SQLiteBrowser.Experiment.ViewModels
{
    public class Property
    {
        public ColumnHeader ColumnHeader { get; set; }  
        public Type Type => Value.GetType();
        public int Thing { get; set; }
        public object Value { get; set; }
        public string Text => Value.ToString();
    }
}