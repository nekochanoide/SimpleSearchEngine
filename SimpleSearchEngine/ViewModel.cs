using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace SimpleSearchEngine
{
    class ViewModel : PropertyChangedBase
    {
        private ImageSource background;
        private UserQueryModel userQueryModel;
        private string windowName;
        private ObservableCollection<MovieModel> movies;

        public ImageSource Background { get => background; set => SetField(ref background, value); }
        public UserQueryModel UserQueryModel { get => userQueryModel; set => SetField(ref userQueryModel, value); }
        public ObservableCollection<MovieModel> Movies { get => movies; set => SetField(ref movies, value); }
        public string WindowName { get => windowName; set => SetField(ref windowName, value); }
        public Dispatcher Dispatcher { get; set; }

        public ViewModel()
        {
            UserQueryModel = new UserQueryModel();
#if egg
            UserQueryModel.CultFound += UserQueryModel_CultFound;
#endif
            UserQueryModel.PropertyChanged += UserQueryModel_PropertyChanged;
            Movies = new ObservableCollection<MovieModel>();
            Dispatcher = Dispatcher.CurrentDispatcher;
        }

        private async void UserQueryModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(UserQueryModel.SearchField))
                return;
            //string connstring = "Server=127.0.0.1; Port=5432; User Id=postgres; Password=1; Database=simplesearchengine;";
            string connstring = "Server=127.0.0.1; " +
                "Port=5432; " +
                "User Id=postgres; " +
                "Password=1; " +
                "Database=simplesearchengine;";
            var connection = new NpgsqlConnection(connstring);
            connection.Open();
            var commandText = SqlCommandBuilder.BuildByFts(UserQueryModel.ConvertedToTsQuery);
            NpgsqlCommand command = new NpgsqlCommand(commandText, connection);
            NpgsqlDataReader dataReader = command.ExecuteReader();

            await QueryDB(dataReader); //next is to find she sho

            if (Movies.Count != 0)
            {
                connection.Close();
                return;
            }

            commandText = SqlCommandBuilder.BuildByIlike(UserQueryModel.Words);
            command = new NpgsqlCommand(commandText, connection);
            dataReader = command.ExecuteReader();

            await QueryDB(dataReader);
            connection.Close();
        }

        private Task QueryDB(NpgsqlDataReader dataReader)
        {
            return Task.Factory.StartNew(() =>
            {
                Dispatcher.Invoke(() =>
                {
                    Movies = new ObservableCollection<MovieModel>();
                });
                while (dataReader.Read())
                {
                    var movie = new MovieModel
                    {
                        Id = (int)dataReader["id"],
                        Name = (string)dataReader["name"],
                        Year = (int)dataReader["year"]
                    };
                    Dispatcher.Invoke(() =>
                    {
                        Movies.Add(movie);
                    });
                }
                dataReader.Close();
            });
        }

#if egg
        private void UserQueryModel_CultFound(object sender, string e)
        {
            var stream = RequerstImage(e);
            Background = GetBitmapImage(stream);
        }

        private static Stream RequerstImage(string query)
        {
            WebRequest rq = WebRequest.Create("https://source.unsplash.com/1600x900/?" + query);
            return rq.GetResponse().GetResponseStream();
        }

        public static BitmapImage GetBitmapImage(Stream stream)
        {
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = stream;
            bitmapImage.EndInit();
            return bitmapImage;
        }
#endif
    }
}
