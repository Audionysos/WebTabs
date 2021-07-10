Little project for saving list of all opened tabs in a window and opening them back from windows explorer.

##### The project consist of:

- Chrome browser extension (currently available only in browser's developer mode).
  - Stores title and addresses of web pages.
  - Tested working with Google Chrome and Brave
- .NET Core 3.1 console application responsible for opening browsers from saved tabs files. 
  - File configuration of target browser which suppose to open the tabs.
  - Optional preview of tabs to be opened with required confirmation.

##### TODO:

- ***** In browser has not any windows already opened, the tabs will be add to the tabs of previously closed window (found in brave, chrome probably affected to)
- Figure out how to get rid of this annoying puzzle icon ![](docs/puzzle_icon.PNG)
- ***** Search further if it's possible to specify name and path of saved file (without changing default download behavior)
  - Would been also nice to be able to append to a file
- Check how to compile/pack the chrome extension.
- Write docs for the configuration.
- Convert icon to support multiple sizes (current icon looks really bad when small) 