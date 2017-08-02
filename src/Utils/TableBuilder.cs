using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace HockeyApp.Utils
{
    public class TableBuilder : IEnumerable<ITextRow>
    {
        protected string FmtString;
        protected List<int> ColLength = new List<int>();

        protected List<ITextRow> Rows = new List<ITextRow>();
        private bool _hasTitleRow;

        public TableBuilder()
        {
            Separator = "  ";
        }

        public TableBuilder(string separator)
            : this()
        {
            Separator = separator;
        }

        public string Separator { get; set; }

        public string FormatString
        {
            get
            {
                if (FmtString != null) return FmtString;
                var i = 0;
                var format = ColLength.Aggregate("", (current, len) => current + $"{{{i++},-{len}}}{Separator}");
                format += "\r\n";
                FmtString = format;
                return FmtString;
            }
        }

        public ITextRow AddRow(params object[] cols)
        {
            var row = new TextRow(this);
            foreach (var str in cols.Select(o => o?.ToString().Trim()??""))
            {
                row.Add(str);
                if (ColLength.Count >= row.Count)
                {
                    var curLength = ColLength[row.Count - 1];
                    if (str.Length > curLength) ColLength[row.Count - 1] = str.Length;
                }
                else
                {
                    ColLength.Add(str.Length);
                }
            }
            Rows.Add(row);
            return row;
        }

        public void WriteToConsole(TextWriter @out)
        {
            var sb = new StringBuilder();
            foreach (var row in Rows.Cast<TextRow>())
            {
                row.Output(sb);
            }
            @out.WriteLine(sb.ToString());
        }



        #region IEnumerable Members

        public IEnumerator<ITextRow> GetEnumerator()
        {
            return Rows.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Rows.GetEnumerator();
        }

        #endregion

        public void AddTitleRow(params object[] cols)
        {
            if (_hasTitleRow)
            {
                throw new Exception("Table already has a Title Row");
            }
            _hasTitleRow = true;
            var underlines = new object[cols.Length];
            for(var i=0;i<cols.Length; i++)
            {
                underlines[i] = new string('-', cols[i].ToString().Length);
            }
            AddRow(cols);
            AddRow(underlines);
        }
    }
}
