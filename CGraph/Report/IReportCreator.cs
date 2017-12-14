using System.IO;

namespace CGraph.Report
{
    internal interface IReportCreator
    {
        void Create(Stream outputStream);
    }
}
