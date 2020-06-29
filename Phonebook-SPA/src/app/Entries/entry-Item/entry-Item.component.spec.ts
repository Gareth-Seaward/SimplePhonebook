/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { EntryItemComponent } from './Entry-Item.component';

describe('EntryItemComponent', () => {
  let component: EntryItemComponent;
  let fixture: ComponentFixture<EntryItemComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EntryItemComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EntryItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
