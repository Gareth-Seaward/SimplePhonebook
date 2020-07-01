import { TestBed, async } from '@angular/core/testing';
import { AppComponent } from './app.component';
import { PhonebookComponent } from './phonebook/phonebook.component';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { EntiryListComponent } from './entries/entiry-list/entiry-list.component';
import { EntryNewComponent } from './Entries/entry-new/entry-new.component';
import { RouterModule } from '@angular/router';
import { appRoutes } from './routes';
import { AuthService } from './_services/auth.service';

describe('AppComponent', () => {
  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [
        AppComponent,
        PhonebookComponent,
        AppComponent,
        HomeComponent,
        RegisterComponent,
        EntiryListComponent,
        EntiryListComponent,
        EntryNewComponent
      ],
      imports: [
        RouterModule.forRoot(appRoutes),
      ],
      providers: [
      ]
    }).compileComponents();
  }));

  it('should create the app', () => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.debugElement.componentInstance;
    expect(app).toBeTruthy();
  });

  it(`should have as title 'Phonebook-SPA'`, () => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.debugElement.componentInstance;
    expect(app.title).toEqual('Phonebook-SPA');
  });

  it('should have a app-nav element', () => {
    const fixture = TestBed.createComponent(AppComponent);
    fixture.detectChanges();
    const compiled = fixture.debugElement.nativeElement();
    expect(compiled.querySelector('app-nav')).toBeDefined();
  })
});
