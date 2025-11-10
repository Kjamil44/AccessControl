import { Component, Inject, OnInit, ViewChildren, QueryList, ElementRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators, AbstractControl } from '@angular/forms';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { AccessControlService } from 'src/app/services/access-control.service';

@Component({
  selector: 'app-lock-unlock',
  templateUrl: './lock-unlock.component.html',
  styleUrls: ['./lock-unlock.component.css'],
  standalone: false
})
export class LockUnlockComponent implements OnInit {
  form!: FormGroup;
  toggleChanged = false;
  formSubmitted = false;

  lock!: any;      // provided via dialog data

  @ViewChildren('digitInput') digitInputs!: QueryList<ElementRef<HTMLInputElement>>;

  constructor(
    private fb: FormBuilder,
    public dialogref: DynamicDialogRef,
    public config: DynamicDialogConfig,
    private accessService: AccessControlService
  ) { }

  ngOnInit(): void {
    this.lock = this.config.data?.lock;

    const initState = this.lock?.isLocked ? 'Locked' : 'Unlocked';

    this.form = this.fb.group({
      lockState: [initState, Validators.required],
      d1: [{ value: '', disabled: true }, [Validators.required, Validators.pattern(/^\d$/)]],
      d2: [{ value: '', disabled: true }, [Validators.required, Validators.pattern(/^\d$/)]],
      d3: [{ value: '', disabled: true }, [Validators.required, Validators.pattern(/^\d$/)]],
      d4: [{ value: '', disabled: true }, [Validators.required, Validators.pattern(/^\d$/)]],
      d5: [{ value: '', disabled: true }, [Validators.required, Validators.pattern(/^\d$/)]],
      d6: [{ value: '', disabled: true }, [Validators.required, Validators.pattern(/^\d$/)]],
    });
  }

  onToggleChanged(): void {
    if (!this.toggleChanged) {
      this.toggleChanged = true;
      ['d1', 'd2', 'd3', 'd4', 'd5', 'd6'].forEach(c => this.form.get(c)?.enable());
    }
  }

  autoAdvance(evt: Event, nextControlName: string | null): void {
    const input = evt.target as HTMLInputElement;
    const val = input.value.replace(/\D/g, '').slice(0, 1);
    input.value = val;
    const ctrl = this.form.get(this.findControlNameByElement(input));
    if (ctrl && ctrl.value !== val) { ctrl.setValue(val); }

    if (val && nextControlName) {
      const next = this.getControlElement(nextControlName);
      next?.focus();
    }
  }

  private findControlNameByElement(el: HTMLInputElement): string {
    // map DOM element to control by matching its formControlName attribute
    return el.getAttribute('formControlName') || '';
  }

  private getControlElement(name: string): HTMLInputElement | null {
    // query by attribute selector (works without template refs)
    return document.querySelector(`input[formControlName="${name}"]`) as HTMLInputElement | null;
  }

  onBack(): void {
    this.dialogref.close();
  }

  onConfirm(): void {
    this.formSubmitted = true;

    if (this.form.invalid || !this.toggleChanged) return;

    const desiredState = this.form.value.lockState === 'Locked' ? 'lock' : 'unlock'; 

    const cardNumber = [
      this.form.value.d1, this.form.value.d2, this.form.value.d3,
      this.form.value.d4, this.form.value.d5, this.form.value.d6
    ].join('');

    const request = {
      cardNumber: cardNumber,
      momentaryTriggerDate: new Date().toISOString()
    };

    this.accessService.updateAction(`api/locks`, this.lock.lockId, desiredState, request).subscribe({
      next: data => {
        this.accessService.createSuccessNotification(`${this.lock.displayName} ${data.isLocked ? 'locked' : 'unlocked'} successfully!`)
        this.closeLockUnlockDialog();
      },
      error: error => {
        this.accessService.createErrorNotification(error.message)
        this.closeLockUnlockDialog();
      }
    })
  }

  closeLockUnlockDialog() {
    this.dialogref.close();
  }
}
