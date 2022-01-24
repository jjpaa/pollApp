using SimpleHttp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net;

namespace DotNetBackend
{
    class SimpleHttpServer
    {
        public static void Server(SqliteDataAccess sda)
        {
            int port = 8081;
            Console.WriteLine("Server Started");
            Console.WriteLine("Port " + port);

            Route.Add("/", (req, res, props) =>
            {
                res.AsText("This is simple .NET server");
                res.AddHeader("Access-Control-Allow-Origin", "*");
            });

            Route.Add("/polls", (req, res, props) =>
            {
                List<Poll> l = new List<Poll>();
                l = sda.LoadPolls();
                Dictionary<string, List<Poll>> d = new Dictionary<string, List<Poll>>();
                d.Add("polls", l);
                string jsonString = JsonSerializer.Serialize(d);
                byte[] buffer = Encoding.UTF8.GetBytes(jsonString);
                // Get a response stream and write the response to it.
                res.ContentLength64 = buffer.Length;
                res.ContentType = "application/json";
                res.AddHeader("Access-Control-Allow-Origin", "*");
                System.IO.Stream output = res.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                output.Close();
            });

            Route.Add("/polls/{id}/vote/{option}", (req, res, props) =>
            {
                int.TryParse(props["id"], out int pollId);
                int.TryParse(props["option"], out int optionId);
                sda.Vote(pollId, optionId);



                // From here it is same as in the get poll with ID
                    string title = "";
                    int.TryParse(props["id"], out int id);
                    var objDictionary = new Dictionary<string, object>();

                    List<Poll> l = new List<Poll>();
                    l = sda.LoadPollWithId(id);

                    foreach (Poll poll in l)
                    {
                        title = poll.title;
                    }

                    objDictionary.Add("id", id);
                    objDictionary.Add("title", title);

                    List<Option> o = new List<Option>();
                    o = sda.LoadOptionsWithPollId(id);
                    objDictionary.Add("options", o);

                    string jsonString = JsonSerializer.Serialize(objDictionary);
                    byte[] buffer = Encoding.UTF8.GetBytes(jsonString);
                    // Get a response stream and write the response to it.
                    res.ContentLength64 = buffer.Length;
                    res.ContentType = "application/json";
                    res.AddHeader("Access-Control-Allow-Origin", "*");
                    System.IO.Stream output = res.OutputStream;
                    output.Write(buffer, 0, buffer.Length);
                    output.Close();
                // To here it is same // Should clean it later :)

            }, "POST");

            Route.Add("/polls/{id}", (req, res, props) =>
            {
                string title = "";
                int.TryParse(props["id"], out int id);
                var objDictionary = new Dictionary<string, object>();

                List<Poll> l = new List<Poll>();
                l = sda.LoadPollWithId(id);

                foreach (Poll poll in l)
                {
                    title = poll.title;
                }

                objDictionary.Add("id", id);
                objDictionary.Add("title", title);

                List<Option> o = new List<Option>();
                o = sda.LoadOptionsWithPollId(id);
                objDictionary.Add("options", o);

                string jsonString = JsonSerializer.Serialize(objDictionary);
                byte[] buffer = Encoding.UTF8.GetBytes(jsonString);
                // Get a response stream and write the response to it.
                res.ContentLength64 = buffer.Length;
                res.ContentType = "application/json";
                res.AddHeader("Access-Control-Allow-Origin", "*");
                System.IO.Stream output = res.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                output.Close();
            });

            Route.Add("/polls/add", (req, res, props) =>
            {
                System.IO.Stream body = req.InputStream;
                System.Text.Encoding encoding = req.ContentEncoding;
                System.IO.StreamReader reader = new System.IO.StreamReader(body, encoding);
                string s = reader.ReadToEnd();
                body.Close();
                reader.Close();

                var objDictionary = new Dictionary<string, object>();
                objDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(s);
                string pollTitle = objDictionary.Values.First().ToString();

                // Insert Poll into Polls           // Add "" around text
                sda.InsertInto("polls", "title", '"' + pollTitle + '"');

                // Discard first element
                objDictionary.Remove(objDictionary.Keys.First());

                // Get first element values and parse into json string
                string jsonString = JsonSerializer.Serialize(objDictionary.Values.First());

                // Remove \ from the strings
                jsonString.Replace(@"\", string.Empty);

                // remove [] around the string
                jsonString = jsonString.Substring(1, jsonString.Length - 2);

                //Make into string array
                string[] options = jsonString.Split(',');

                int pollID = sda.SQLGetPollID("SELECT MAX(id) from polls");

                for (int i = 1; i <= options.Length; i++)
                {
                    string option = options[i - 1];

                    //Insert Option to Options
                    sda.InsertInto("options",
                        "id, title, votes, pollId",
                        i + ", " + option + ", " + "0, " + pollID);
                }

                    // From here it is pretty much the same as in the get poll with ID
                    string title = "";
                    int id = pollID;
                    var objDictionary2 = new Dictionary<string, object>();

                    List<Poll> l = new List<Poll>();
                    l = sda.LoadPollWithId(id);

                    foreach (Poll poll in l)
                    {
                        title = poll.title;
                    }

                    objDictionary2.Add("id", id);
                    objDictionary2.Add("title", title);

                    List<Option> o = new List<Option>();
                    o = sda.LoadOptionsWithPollId(id);
                    objDictionary2.Add("options", o);

                    string jsonString2 = JsonSerializer.Serialize(objDictionary2);
                    byte[] buffer = Encoding.UTF8.GetBytes(jsonString2);
                    // Get a response stream and write the response to it.
                    res.WithCORS();
                    res.ContentLength64 = buffer.Length;
                    res.ContentType = "application/json";
                    res.AddHeader("Access-Control-Allow-Origin", "*");
                    res.AddHeader("Access-Control-Allow-Headers", "*");
                    System.IO.Stream output = res.OutputStream;
                    output.Write(buffer, 0, buffer.Length);
                    output.Close();
                    // To here it is same // Should clean it later :)


            }, "POST");

            HttpServer.ListenAsync(
                    port,
                    CancellationToken.None,
                    Route.OnHttpRequestAsync
                )
                .Wait();
        }
    }
}
