using System.Text;

namespace HockeyApp.Utils
{
    public interface ITextRow
    {
        string Output();
        void Output(StringBuilder sb);
        object Tag { get; set; }
    }
}