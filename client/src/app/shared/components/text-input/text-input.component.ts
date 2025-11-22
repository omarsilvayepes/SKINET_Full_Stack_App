import { Component, Input, Self } from '@angular/core';
import { ControlValueAccessor, FormControl, NgControl, ReactiveFormsModule } from '@angular/forms';
import { MatFormField, MatLabel } from '@angular/material/form-field';
import { MatInput } from '@angular/material/input';
import { MatError } from '@angular/material/form-field';

@Component({
  selector: 'app-text-input',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatFormField,
    MatInput,
    MatError,
    MatLabel,
  ],
  templateUrl: './text-input.component.html',
  styleUrl: './text-input.component.scss'
})
export class TextInputComponent implements ControlValueAccessor {
  @Input() label = '';
  @Input() type = 'text';

  value: any;
  onChange = (_: any) => {};
  onTouched = () => {};

  constructor(@Self() public controlDir: NgControl) {
    this.controlDir.valueAccessor = this;
  }

  writeValue(obj: any): void {
    this.value = obj;
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  setDisabledState?(isDisabled: boolean): void {
    if (isDisabled) {
      this.control?.disable();
    } else {
      this.control?.enable();
    }
  }

  get control() {
    return this.controlDir.control as FormControl;
  }

  onInput(event: Event): void {
  const input = event.target as HTMLInputElement;
  this.onChange(input.value);
}
}
