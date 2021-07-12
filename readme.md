Little project for saving list of all opened tabs in a window and opening them back from windows explorer.

##### The project consist of:

- **Browser extensions** (currently available only in browser's developer mode).
  - Stores title and addresses of web pages.
  - Tested working with Google Chrome, Brave and Firefox
- .NET Core 3.1 **console application** responsible for opening browsers from saved tabs files. 
  - File configuration of target browser which suppose to open the tabs.
  - Optional preview of tabs to be opened with required confirmation.
  - Due to using of windows registry API for associating ".tabs" files with the program, the program currently **works only on Windows**. 

#### Installation

The project currently don't have any installer or releases so you need to install it manually, but it's quite straightforward. 

1. Clone or download this repository

2. Install extension for your browser. In **browserExtensions** folder you will find extensions for the web browsers.

   ##### Installing extension for Google Chrome (v91), Brave (or possibly other similar browsers):

   - Enter `chrome://extensions/` in your browser's navigation bar.
   - Locate a **Developer Mode** switch and turn it on.
   - Now a **Load unpacked** button should show up, click it and open `browserExtensions\chrome` folder of your repository.
   - The extension should show up on the list and you should see the extension icon (puzzle) on the right of your navigation bar. Click it and then click "Tabs to file saver" to save the tabs of current window. You may want to "pin" this button to show up next to navigation bar as well.

   ##### Installing extension for Firefox (89.0.2):

   Note that Firefox will automatically remove all temporary extensions (that are not submitted to Firefox) when you close all of it's windows, so you would need to repeat this steps after restarting the browser.

   - Enter `about:debugging` in your browser's navigation bar.
   - Click **This Firefox** and click **Load Temporary Add-on** button.
   - Select any file inside `browserExtensions\firefox` folder
   - The extension should show up on the list and you should see the extension icon (puzzle) on the right of your navigation bar. Click it to save the tabs of current window.

   In all browsers, saved file shows up as a downloaded file (named "group.tabs" by default) (like when you download other files from the internet). Chrome and Brave for some reason don't accept file extension that is specified by the plugin and they will change ".tabs" to ".txt". You should change extension of the file manually if you want it to be opened by the program.

3. Compile the "Web Tabs Opener" program.

   - You should have at least [.NET Core 3.1](https://dotnet.microsoft.com/download) installed to compile.
   - Go to root of the repository and enter `dotnet build` command.

4. Configure the program.

   - Navigate to `WebTabsOpener\bin\Debug\netcoreapp3.1` folder
   - Start the program. It should create **config** file in the same directory and ask you for associating ".tabs" files.
   - Open the **config** file and paste path to your browser's .exe file.  

##### TODO:

- ***** In browser has not any windows already opened, the tabs will be add to the tabs of previously closed window (found in brave, chrome probably affected to)
- Firefox opens first tab from list in a separated window
- Figure out how to get rid of this annoying puzzle icon ![](docs/puzzle_icon.PNG)
- ***** Search further if it's possible to specify name and path of saved file (without changing default download behavior)
  - Would been also nice to be able to append to a file
- Check how to compile/pack the chrome extension.
- Submit browsers extensions and add auto installation.
- Convert icon to support multiple sizes (current icon looks really bad when small) 