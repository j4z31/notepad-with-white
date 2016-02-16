using NUnit.Framework;
using System;
using System.IO;
using TestStack.White;
using TestStack.White.InputDevices;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.WindowsAPI;

namespace Tests
{
    [TestFixture]
    public class SaveFile
    {
        private string folderPath;
        private Application notepadApplication;
        private Window notepadWindow;

        [SetUp]
        public void initialize()
        {
            folderPath = Constant.FOLDER_PATH;
            Directory.CreateDirectory(folderPath);

            notepadApplication = Application.Launch(Constant.NOTEPAD_PATH);
            notepadWindow = notepadApplication.GetWindow(Constant.TITLE_DOCUMENT);
        }

        [Test]
        public void saveTheFile()
        {
            SearchCriteria txtEditSearch = SearchCriteria.ByAutomationId(Constant.ID_TXT_TEXT);
            var txtEdit = notepadWindow.Get(txtEditSearch);
            Keyboard.Instance.Send(Constant.TEXT, txtEdit);

            notepadWindow.MenuBar.MenuItem(Constant.MENU_FILE, Constant.SUBMENU_SAVE_AS).Click();

            //Click on the address bar
            SearchCriteria btnSearchLocation = SearchCriteria.ByAutomationId(Constant.BTN_SEARCH_LOCATION);
            var txtToolbarAddress = notepadWindow.Get(btnSearchLocation);
            txtToolbarAddress.Click();
            Keyboard.Instance.Send(folderPath, txtToolbarAddress);

            //Enter to path
            notepadWindow.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.RETURN);

            // Click on the file name
            SearchCriteria fileNameSearch = SearchCriteria.ByAutomationId(Constant.ID_FILENAME_SEARCH).AndByClassName(Constant.CLASSNAME_FILENAME_SEARCH);
            var txtFileName = notepadWindow.Get(fileNameSearch);
            txtFileName.DoubleClick();
            notepadWindow.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.BACKSPACE);
            Keyboard.Instance.Send(Constant.FILENAME, txtFileName);

            notepadWindow.Get(SearchCriteria.ByText(Constant.BTN_SAVE)).Click();

            Assert.True((File.Exists(folderPath + Constant.FILENAME) ? true : false), Constant.MESSAGE_FILE);
        }

        [TearDown]
        public void clean()
        {
            //notepadApplication.Kill();
            notepadApplication.Close();
            Directory.Delete(folderPath, true);
        }
    }
}
