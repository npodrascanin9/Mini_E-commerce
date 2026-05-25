import { Injectable } from "@angular/core";
import { DownloadFileArgs } from "../models/downloadFileArgs.model";

@Injectable({
    providedIn: 'root'
})

export class FileExporterService {

    exportFile(file: any): void {
        this.downloadFile({
            fileContents: file?.fileContents,
            fileDownloadName: file?.fileDownloadName,
            contentType: file?.contentType
        });
    }

    private downloadFile(args: DownloadFileArgs): void {
        const link = document.createElement('a');
        [link.href, link.download] = [`data:application/${args.contentType};base64,${args.fileContents}`, args.fileDownloadName];
        link.dispatchEvent(this._mouseEvent());
    }

    private _mouseEvent(): MouseEvent {
        return new MouseEvent(
            'click', 
            this._mouseEventArgs());
    }

    private _mouseEventArgs(): MouseEventInit {
        return {
            bubbles: true,
            cancelable: true,
            view: window
        };
    }
}
