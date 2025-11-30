import { AfterViewInit, Component, ElementRef, inject, ViewChild } from '@angular/core';
import SignaturePad from 'signature_pad';
import { AccountService } from '../../../core/services/account.service';
import { SnackbarService } from '../../../core/services/snackbar.service';

@Component({
  selector: 'app-add-signature',
  imports: [],
  templateUrl: './add-signature.component.html',
  styleUrl: './add-signature.component.scss'
})
export class AddSignatureComponent implements AfterViewInit {
  @ViewChild('signatureCanvas') signatureCanvas!: ElementRef<HTMLCanvasElement>;

  private signaturePad!: SignaturePad;
  private acountService = inject(AccountService);
  private snackbarService = inject(SnackbarService);
  
  clinicId = this.acountService.currentUser()?.clinicId;
  isSaving = false;

  ngAfterViewInit(): void {
    const canvas = this.signatureCanvas.nativeElement;

    const ratio = Math.max(window.devicePixelRatio || 1, 1);
    canvas.width = canvas.offsetWidth  * ratio;
    const height = canvas.offsetHeight * ratio;
    canvas.getContext('2d')?.scale(ratio, ratio);

    this.signaturePad = new SignaturePad(canvas, {
      penColor: '#000000',
      backgroundColor: 'rgba(0,0,0,)'
    });
  }

  clearSignature() {
    this.signaturePad.clear();
  }

  async saveSignature() {
    if (this.signaturePad.isEmpty()) {
      this.snackbarService.error("Please draw you signature first");
      return;
    }

    this.isSaving = true;

    const dataURL = this.signaturePad.toDataURL('image/png');

    const blob = await this.dataURLtoBlob(dataURL);
    const file = new File([blob], "singature.png", {type: "image/png"});

    const formData = new FormData();
    formData.append("file", file);
  }

  dataURLtoBlob(dataURL: string): Promise<Blob> {
    return fetch(dataURL).then(res => res.blob());
  }
}
