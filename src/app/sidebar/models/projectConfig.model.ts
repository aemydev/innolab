import{v4 as uuidv4} from 'uuid';
import { Moment } from 'moment';
import * as moment from 'moment';

type GUID = string & { isGuid: true};

export class ProjectConfig{
    ID : GUID; //Guid
    ProjectName: string;
    CreatedAt:Moment; // date time?
    LastOpened: Moment;
    Path:string;
    ContentHash: string; // Hash
    
    constructor(projectName: string, path:string, contentHash: string){
        this.ID = uuidv4();
        this.ProjectName = projectName;
        this.CreatedAt = moment(new Date(),'YYYY-MM-DDTHH:mm');
        this.LastOpened = moment(new Date(),'YYYY-MM-DDTHH:mm');
        this.Path = path;
        this.ContentHash=contentHash;
    }
}


