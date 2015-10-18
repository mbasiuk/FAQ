using Faq.Library.Properties;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Faq.Library
{
    /// <summary>
    /// Represent a Faq
    /// </summary>
    public sealed class Faq
    {
        private Faq(int id, string question, string answer)
        {
            Id = id;
            Answer = answer;
            Question = question;
        }

        public Faq(string question, string answer)
        {
            Answer = answer;
            Question = question;
        }

        /// <summary>
        /// Summary
        /// </summary>
        public string Question
        {
            get; set;
        }

        /// <summary>
        /// Answer
        /// </summary>
        public string Answer
        {
            get; set;
        }

        private int Id
        {
            get; set;
        }

        #region Database

        /// <summary>
        /// Search Faq in database
        /// </summary>
        public static List<Faq> FindFaq(string pattern)
        {
            List<Faq> result = new List<Faq>();
            using (var connection = new SqlConnection(Settings.Default.connectionString))
            {
                using (var command = new SqlCommand(Resources.FindFaq, connection))
                {
                    command.Parameters.AddWithValue("Pattern", pattern);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Faq faq = Convert(reader);
                            result.Add(faq);
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Load all Faq from database
        /// </summary>
        public static List<Faq> GetAllFaq()
        {
            List<Faq> result = new List<Faq>();
            using (var connection = new SqlConnection(Settings.Default.connectionString))
            {
                using (var command = new SqlCommand(Resources.GetAllFaq, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Faq faq = Convert(reader);
                            result.Add(faq);
                        }
                    }
                }
            }
            return result;
        }

        public void InsertFaq()
        {
            using (var connection = new SqlConnection(Settings.Default.connectionString))
            {
                using (var command = new SqlCommand(Resources.InsertFaq, connection))
                {
                    command.Parameters.AddWithValue("answer", Answer);
                    command.Parameters.AddWithValue("question", Question);
                    connection.Open();
                    Id = (int)(decimal)command.ExecuteScalar();
                }
            }
        }

        public void Delete()
        {
            using (var connection = new SqlConnection(Settings.Default.connectionString))
            {
                if (Id == 0)
                {
                    using (var command = new SqlCommand(Resources.DeleteByQuestionAndAnswer, connection))
                    {
                        command.Parameters.AddWithValue("question", Question);
                        command.Parameters.AddWithValue("answer", Answer);
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
                else
                {
                    using (var command = new SqlCommand(Resources.DeleteByID, connection))
                    {
                        command.Parameters.AddWithValue("id", Id);
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        private static Faq Convert(SqlDataReader reader)
        {
            int id = reader.GetInt32(0);
            string answer = reader.GetString(1);
            string question = reader.GetString(2);
            Faq faq = new Faq(id, question, answer);
            return faq;
        }

        #endregion
    }
}
