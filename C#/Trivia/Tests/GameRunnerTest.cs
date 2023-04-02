using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Runtime.ConstrainedExecution;
using Trivia;

namespace Test { 
    [TestClass]
    public class GameRunnerTest
    {
        // le dossier où se trouvent les fichiers à traiter
        readonly string path = "../../../resource/";

        [TestMethod]
        public void assertMainResult()
        {
            // expected : la sortie générée au lancement du GameRunner sans modification
            // output : la nouvelle sortie à générer chaque fois que l'on modifie le code
            Assert.AreEqual(File.ReadAllText(path + "expected.txt"), File.ReadAllText(path + "output.txt"));
        }
    }
}
