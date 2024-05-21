using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Data
{
    public class BallsLogger
    {
        private BlockingCollection<string> fifoStackCollection;
        StreamWriter writer;

        public BallsLogger(string path)
        {
            fifoStackCollection = new BlockingCollection<string>();
            FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);

            writer = new StreamWriter(fileStream);
            
            Task.Run(startLogging);
            
        }

        ~BallsLogger()
        {
            writer.Close();
        }

        private void startLogging()
        {
            foreach (var item in fifoStackCollection.GetConsumingEnumerable())
            {
                writer.WriteLine(item);
            }
        }

        public void makeLog(string log)
        {
            var logEntry = new
            {
                Timestamp = DateTime.Now.ToString("F"),
                Message = log
            };

            string jsonLog = JsonConvert.SerializeObject(logEntry);
            fifoStackCollection.Add(jsonLog);
        }
    }
}