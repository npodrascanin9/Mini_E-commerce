export class DownloadFileArgs {
    fileContents: any[];
    fileDownloadName: string;
    contentType: string;

    constructor(file: any) {
        this.fileContents = file?.fileContents;
        this.fileDownloadName = file?.fileDownloadName;
        this.contentType = file?.contentType;
    }
}