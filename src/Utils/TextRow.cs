using System;
using System.Collections.Generic;
using System.Text;

namespace HockeyApp.Utils
{
    public class TextRow : List<string>, ITextRow
    {
        protected TableBuilder Owner;

        public TextRow(TableBuilder owner)
        {
            this.Owner = owner;
            if (this.Owner == null) throw new ArgumentException("Owner");
        }

        public string Output()
        {
            var sb = new StringBuilder();
            Output(sb);
            return sb.ToString();
        }

        public void Output(StringBuilder sb)
        {
            sb.AppendFormat(Owner.FormatString, ToArray());
        }

        public object Tag { get; set; }
    }
}