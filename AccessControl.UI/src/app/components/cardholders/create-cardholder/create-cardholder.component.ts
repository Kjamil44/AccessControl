import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { AccessControlService } from 'src/app/services/access-control.service';
import { SpinnerService } from 'src/app/services/spinner.service';

@Component({
  selector: 'app-create-cardholder',
  templateUrl: './create-cardholder.component.html',
  styleUrls: ['./create-cardholder.component.css'],
})
export class CreateCardholderComponent implements OnInit {
  sites: any[] = [];
  formGroup: FormGroup;

  toggleChanged = true;

  constructor(
    private accessService: AccessControlService,
    private dialogref: DynamicDialogRef,
    public spinner: SpinnerService,
    private config: DynamicDialogConfig
  ) {
    this.formGroup = new FormGroup({
      firstName: new FormControl(),
      lastName: new FormControl(),

      // 6 boxes
      d1: new FormControl('', [Validators.pattern(/^\d?$/)]),
      d2: new FormControl('', [Validators.pattern(/^\d?$/)]),
      d3: new FormControl('', [Validators.pattern(/^\d?$/)]),
      d4: new FormControl('', [Validators.pattern(/^\d?$/)]),
      d5: new FormControl('', [Validators.pattern(/^\d?$/)]),
      d6: new FormControl('', [Validators.pattern(/^\d?$/)]),

      // hidden, kept for payload + validation
      cardNumber: new FormControl('', [
        Validators.required,
        Validators.pattern(/^\d{6}$/),
      ]),

      site: new FormControl(),
    });
  }

  ngOnInit(): void {
    this.accessService.get('api/sites').subscribe({
      next: (response) => {
        this.sites = response.data;
      },
      error: (err: Error) => {
        this.accessService.createErrorNotification(err.message);
      },
    });

    this.formGroup.valueChanges.subscribe(() => this.syncDigitsToCardNumber());
  }

  private syncDigitsToCardNumber() {
    const v = this.formGroup.value;
    const digits = [v.d1, v.d2, v.d3, v.d4, v.d5, v.d6].map((x: string) =>
      (x || '').replace(/\D/g, '')
    );
    const merged = digits.join('');
    if (this.formGroup.get('cardNumber')!.value !== merged) {
      this.formGroup.get('cardNumber')!.setValue(merged, { emitEvent: false });
    }
  }

  autoAdvance(e: Event, nextId: string | null) {
    const input = e.target as HTMLInputElement;
    // keep only last typed digit
    input.value = (input.value.match(/\d/) || [''])[0];
    const ctrlName = input.id;
    this.formGroup.get(ctrlName)?.setValue(input.value);

    if (input.value && nextId) {
      const next = document.getElementById(nextId) as HTMLInputElement | null;
      next?.focus();
      next?.select?.();
    }
  }

  onDigitKeydown(
    e: KeyboardEvent,
    prevId: string | null,
    nextId: string | null
  ) {
    const input = e.target as HTMLInputElement;

    // move back on backspace if empty
    if (e.key === 'Backspace' && !input.value && prevId) {
      const prev = document.getElementById(prevId) as HTMLInputElement | null;
      prev?.focus();
      prev?.select?.();
    }

    // allow arrows to move
    if (e.key === 'ArrowLeft' && prevId) {
      e.preventDefault();
      document.getElementById(prevId)?.focus();
    }
    if (e.key === 'ArrowRight' && nextId) {
      e.preventDefault();
      document.getElementById(nextId)?.focus();
    }

    // block non-digits except control keys
    if (e.key.length === 1 && !/^\d$/.test(e.key) && !e.ctrlKey && !e.metaKey) {
      e.preventDefault();
    }
  }

  onPaste(e: ClipboardEvent) {
    const pasted = e.clipboardData?.getData('text') ?? '';
    if (!pasted) return;

    const digits = pasted.replace(/\D/g, '').slice(0, 6).split('');
    if (!digits.length) return;

    e.preventDefault();

    // fill inputs left-to-right
    ['d1', 'd2', 'd3', 'd4', 'd5', 'd6'].forEach((id, idx) => {
      const el = document.getElementById(id) as HTMLInputElement | null;
      const d = digits[idx] ?? '';
      el && (el.value = d);
      this.formGroup.get(id)?.setValue(d);
    });

    // focus last filled (or d6)
    const lastIndex = Math.min(digits.length, 6) - 1;
    const focusId = ['d1', 'd2', 'd3', 'd4', 'd5', 'd6'][
      Math.max(lastIndex, 0)
    ];
    (document.getElementById(focusId) as HTMLInputElement | null)?.focus();
  }

  createCardholder() {
    this.syncDigitsToCardNumber();

    if (this.formGroup.invalid) {
      this.formGroup.markAllAsTouched();
      this.accessService.createErrorNotification(
        'Please enter a 6-digit Card number.'
      );
      return;
    }

    const data = {
      siteId: this.formGroup.value.site.siteId,
      firstName: this.formGroup.value.firstName,
      lastName: this.formGroup.value.lastName,
      cardNumber: this.formGroup.value.cardNumber,
    };

    this.spinner
      .with(this.accessService.create(`api/cardholders`, data))
      .subscribe({
        next: () => {
          this.accessService.createSuccessNotification(
            'Cardholder created successfully!'
          );
          this.closeCreateDialog();
        },
        error: (err: Error) => {
          this.accessService.createErrorNotification(err.message);
          this.closeCreateDialog();
        },
      });
  }

  closeCreateDialog() {
    this.dialogref.close();
  }
}
