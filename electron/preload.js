const {contextBridge, ipcRenderer} = require('electron')

contextBridge.exposeInMainWorld('electronAPI', {
  setTitle: (title) => ipcRenderer.send('set-title', title),
  openFolder: () => ipcRenderer.invoke('dialog:openFolder'),
  openFile:() => ipcRenderer.invoke('dialog:openFile'),
  createFile: (folder, filename, content) => ipcRenderer.invoke('fs:saveFile', folder, filename, content),
  getAllFilesInDir: (path) => ipcRenderer.invoke('fs:allFiles', path),
  readFile: (path) => ipcRenderer.invoke('fs:readFile', path)
})