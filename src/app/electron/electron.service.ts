import { Injectable } from '@angular/core';

export interface IElectronAPI{
    setTitle: (title:string) => Promise<void>
    openFolder: () => Promise<any>,
    openFile: () => Promise<any>,
    createFile: (folder:string, filename:string, content:string) => Promise<any>,
    getAllFilesInDir: (path:string) => Promise<any>,
    readFile:(path:string) => Promise<any>
}

declare global{
    interface Window{
        electronAPI: IElectronAPI
    }
}

@Injectable({
    providedIn: 'root'
})
export class ElectronService{

    constructor(){}

    setWindowTitle(title:string){
        var win:any  = window.electronAPI.setTitle(title);
    }

    async openFolderPathDialog(){
        console.log("[ElectronService]: openfolderPathDialog called")
        let res = await window.electronAPI.openFolder();
        console.log(res);
        return res; 
    }

    async saveFile(folder:string, filename:string, content:string){
        console.log("[ElectronService]: saveFile called")
        await window.electronAPI.createFile(folder, filename, content);
    }

    async getAllFilesInDir(path:string){
        console.log("[ElectronService]: getAllFilesInDir called")
        let res = await window.electronAPI.getAllFilesInDir(path);
        console.log(res);
        return res;
    }

    async openFile(){
        console.log("[ElectronService]: openFile called")
        let res = await window.electronAPI.openFile();
        return res;
    }

    async readFile(path:string){
        console.log("[ElectronService]: readFile called")
        return await window.electronAPI.readFile(path);
    }
}