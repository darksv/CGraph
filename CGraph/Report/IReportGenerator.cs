using System.IO;

namespace CGraph.Report
{
    interface IReportGenerator
    {
        void Generate(StreamWriter output);
    }
}
