using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Runtime.CompilerServices;
using System.Diagnostics.SymbolStore;

namespace PacMan
{
    public class Scoring
    {
        /// <summary>
        /// Have to have a default constructor for the XmlSerializer.Deserialize method
        /// </summary>
        public Scoring() { }

        /// <summary>
        /// Overloaded constructor used to create an object for long term storage
        /// </summary>
        /// <param name="scores"></param>
       
        public Scoring(List<int> scores)
        {  
            this.Score = scores; 
        }

        public List<int> Score { get; set; }



        public void saveScore(bool saving, List<int> scores)
        {
            lock (this)
            {
                if (!saving)
                {
                    saving = true;
                    //
                    // Create something to save
                    Scoring myState = new Scoring(scores);
                    finalizeSaveScoreAsync(myState, saving);
                }
            }
        }


        private async void finalizeSaveScoreAsync(Scoring state, bool saving)
        {
            await Task.Run(() =>
            {
                using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    try
                    {
                        using (IsolatedStorageFileStream fs = storage.OpenFile("PacManHighscores.xml", FileMode.Create))
                        {
                            if (fs != null)
                            {
                                XmlSerializer mySerializer = new XmlSerializer(typeof(Scoring));
                                mySerializer.Serialize(fs, state);
                            }
                        }
                    }
                    catch (IsolatedStorageException)
                    {
                        // Ideally show something to the user, but this is demo code :)
                    }
                }

                saving = false;
            });
        }


        public void loadScores(bool loading)
        {
            lock (this)
            {
                if (!loading)
                {
                    loading = true;
                    finalizeLoadScoresAsync();
                }
            }
        }

        private Scoring m_loadedState = null;

        private async void finalizeLoadScoresAsync()
        {
            await Task.Run(() =>
            {
                using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    try
                    {
                        if (storage.FileExists("PacManHighscores.xml"))
                        {
                            using (IsolatedStorageFileStream fs = storage.OpenFile("PacManHighscores.xml", FileMode.Open))
                            {
                                if (fs != null)
                                {
                                    XmlSerializer mySerializer = new XmlSerializer(typeof(Scoring));
                                    m_loadedState = (Scoring)mySerializer.Deserialize(fs);
                                }
                            }
                        }
                    }
                    catch (IsolatedStorageException)
                    {
                        // Ideally show something to the user, but this is demo code :)
                    }
                }

                //this.loading = false;
            });

        }

        public Scoring getLoadedState()
        {
            return m_loadedState;
        }
    }



}

