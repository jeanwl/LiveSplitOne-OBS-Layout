# LiveSplit One OBS Layout

Customized LiveSplit One setup for OBS

# Description

LiveSplit One being a web page allows overwriting CSS, thus much more customisation possibilities than original LiveSplit.

OBS Browser source also has native opacity channel, which allows transparency while keeping font sharp and consistent (unlike Chroma key).

[TUNIC LiveSplit One AutoSplitter](https://github.com/jeanwll/TUNIC-LiveSplitOne-AutoSplitter) was made alongside this project as an example to overcome absence of global hotkeys and AutoSplitting.

This layout is a personnal proposition, along with some customizable CSS properties.

![Preview](/preview.gif)

Splits appear progressively with an animation;
- current split is easy to compare with current time
- animations highlight reaching new split
- progressive apparition saves space for game visuals

Sum of Best, PB and WR appear in fading cycle below timer.

# Installation

1. Add browser source
    - Set URL https://one.livesplit.org/
    - Set size to full screen (ex: 1920x1080)
    - Apply Custom CSS `@import url('https://jeanwll.github.io/LiveSplitOne-OBS-Layout/style.css');`

2. Interact source ***(OBS browser import/input window might pop behind OBS application)***
    - Right-click/Left-click to toggle menu
    - Import provided layout **LSOlayout.ls1l**, click save
    - Import your splits, click save
    - Update WR and PB values in Layout → Edit

3. ***(Optionnal)*** Add the blurry background
    - Install OBS plugin [StreamFX](https://obsproject.com/forum/resources/streamfx-for-obs%C2%AE-studio.578/)
    - Make sure your game and browser source have the same size
    - Add "Blur" filter on game source
    - Check "Apply a mask", Mask Type: "Source", Source Mask: "your browser source"
    - Set Mask Alpha Filter to 5, and Size to 30
 
 4. ***(Optionnal)*** Set custom CSS properties below the `@import`, those are default values:
 ```css
.layout {
  --bottom: 290;
  --right: 50;
  --scale: 100;
  --max-visible-splits: 20;
  --show-sob: 1;
  --show-pb: 1;
  --show-wr: 1;
  --appearance-duration: 20;
}
```

# Additionnal notes

Exporting splits won't work directly from OBS browser source; if you want to export splits to .lss you can:
1. Copy content from
`%APPDATA%\obs-studio\plugin_config\obs-browser\IndexedDB\https_one.livesplit.org_0.indexeddb.leveldb`
2. Paste to
`%USERPROFILE%\AppData\Local\Google\Chrome\User Data\Default\IndexedDB\https_one.livesplit.org_0.indexeddb.leveldb`
3. Export splits by browsing https://one.livesplit.org/ on chrome

You could keep a clean record of your game by using OBS plugin [Source Record](https://obsproject.com/forum/resources/source-record.1285/).
You can put the filter on a **Source Mirror** of your game if you use the blurry background.

If you wish to make your own Custom CSS, I recommend enabling *OBS remote-debugging-port*.
