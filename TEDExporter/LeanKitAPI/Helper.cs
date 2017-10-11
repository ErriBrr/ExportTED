using LeanKit.API.Client.Library;
using LeanKit.API.Client.Library.Exceptions;
using LeanKit.API.Client.Library.TransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TEDExporter.DTO;
using TEDExporter.Map;
using Board = LeanKit.API.Client.Library.TransferObjects.Board;
using Card = LeanKit.API.Client.Library.TransferObjects.Card;

namespace TEDExporter.LeanKitAPI
{
    class Helper
    {
        readonly ILeanKitApi _api;
        public Helper()
        {
            _api = CreateApiClient(Connexion.hostName, Connexion.emailAddress, Connexion.password);
        }

        private ILeanKitApi CreateApiClient(string hostName, string emailAddress, string password)
        {
            var auth = new LeanKitBasicAuth
            {
                Hostname = hostName,
                Username = emailAddress,
                Password = password
            };

            var api = new LeanKitClientFactory().Create(auth);
            return api;
        }

        public List<BoardListing> GetBoards()
        {
            try
            {
                var boards = _api.GetBoards().ToList();
                return boards;
            }
            catch (LeanKitAPIException e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey(false);
                return null;
            }
        }

        public Board GetBoard(long boardId)
        {
            return _api.GetBoard(boardId);
        }

        public IEnumerable<CardView> Get20Cards(long boardID)
        {
            SearchOptions srchOpt = new SearchOptions()
            {
                SearchInBacklog = true,
                IncludeBacklogOnly = true,
                SearchInBoard = true

            };
            return _api.SearchCards(boardID, srchOpt);
        }

        public bool DoesntExistCard(ExportTED ted)
        {
            SearchOptions srchOpt = new SearchOptions()
            {
                SearchInBacklog = true,
                SearchInBoard = true,
                SearchInRecentArchive = true,
                SearchInOldArchive = true,
                SearchTerm = ted.Numero.ToString()
            };
            long boardID = 0;
            if (ted.SousSysteme == "Prestations FSS")
            {
                boardID = BoardFSS.ID;
            }
            else if (ted.SousSysteme == "eSRC" || ted.Intitule.ToUpper().StartsWith("ESRC"))
            {
                boardID = BoardESrc.ID;
            }
            else if (ted.SousSysteme == "ServicesCommuns")
            {
                return true;
            }
            else if (ted.SousSysteme.StartsWith("Actuaria"))
            {
                boardID = BoardTauri.ID;
            }
            else if (ted.SousSysteme == "Joachim")
            {
                boardID = BoardJOH1.ID;
            }
            else
            {
                return true;
            }
            try
            {
                return !_api.SearchCards(boardID, srchOpt).Any();
            }
            catch (LeanKitAPIException e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey(false);
                return false;
            }
        }

        public void SetCards(long boardID, List<Card> cards)
        {
            try
            {
                Action<object> action3 = (object obj) =>
                {
                    _api.AddCards(boardID, cards);
                };
                Task t3 = new Task(action3, "?");
                t3.RunSynchronously();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.BackgroundColor = ConsoleColor.White;
                Console.WriteLine("Cartes insérées");
                Console.ResetColor();
            }
            catch (LeanKitAPIException e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.BackgroundColor = ConsoleColor.White;
                Console.WriteLine("ERREUR : ");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Console.ResetColor();
            }
        }
    }
}
