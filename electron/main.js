const { app, BrowserWindow, ipcMain, dialog } = require("electron");
const path = require("path");
var fs = require("fs");

function createWindow() {
  const win = new BrowserWindow({
    width: 1920,
    height: 1080,
    webPreferences: {
      preload: path.join(__dirname, "preload.js"),
    },
  });

  win.removeMenu();

  // Open Main Window -> rel Path?
  win.loadFile(
    "C:\\Users\\chris\\Desktop\\inno.frontend\\dist\\inno.frontend\\index.html"
  );

  // Enable Dev Tools
  //win.webContents.openDevTools();
}

app.whenReady().then(() => {
  createWindow();

  app.on("activate", () => {
    if (BrowserWindow.getAllWindows().length === 0) {
      createWindow();
    }
  });

  ipcMain.on("set-title", (event, title) => {
    const webContents = event.sender;
    const win = BrowserWindow.fromWebContents(webContents);
    win.setTitle(title);
  });

  ipcMain.handle("dialog:openFolder", handleDirOpen);

  ipcMain.handle("dialog:openFile", handleFileOpen);

  ipcMain.handle("fs:saveFile", (event, folder, filename, content) => {
    fs.writeFile(path.join(folder, filename), content, function (err) {
      if (err) {
        console.log(err);
      }
    });
  });

  ipcMain.handle("fs:allFiles", (event, dirPath) => {
    let res = handleGetAllFiles(dirPath);
    console.log(res);
    return res;
  });

  ipcMain.handle("fs:readFile", (event, filename) => {
    try {
      const data = fs.readFileSync(filename, "utf8");
      return data;
    } catch (err) {
      return;
    }
  });
});

app.on("window-all-closed", () => {
  if (process.platform !== "darwin") {
    app.quit();
  }
});

/* ============================================= */
/* Functions */
/* ============================================= */

async function handleDirOpen() {
  const { canceled, filePaths } = await dialog.showOpenDialog({
    properties: ["openDirectory"],
  });
  if (canceled) {
    return;
  } else {
    return filePaths[0];
  }
}

async function handleFileOpen() {
  const { canceled, filePaths } = await dialog.showOpenDialog({
    properties: ["openFile"],
  });
  if (canceled) {
    return;
  } else {
    return filePaths[0];
  }
}

function handleGetAllFiles(dirPath) {
  const files = fs.readdirSync(dirPath, { withFileTypes: true });
  const res = [];

  files.forEach((item) => {
    console.log(item);
    if (item.name != "project.json") {
      if (item.isDirectory()) {
        res.push({
          filename: item.name,
          path: path.join(dirPath, item.name),
          type: "folder",
          content: handleGetAllFiles(path.join(dirPath, item.name)),
        });
      } else {
        res.push({
          filename: item.name,
          path: path.join(dirPath, item.name),
          type: "file",
        });
      }
    }
  });

  return res;
}
