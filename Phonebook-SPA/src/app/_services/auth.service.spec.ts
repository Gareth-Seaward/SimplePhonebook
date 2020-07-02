/* tslint:disable:no-unused-variable */

import { TestBed, async, inject, fakeAsync, tick } from '@angular/core/testing';
import { AuthService } from './auth.service';
import { HttpClientModule } from '@angular/common/http';
import {
  HttpClientTestingModule,
  HttpTestingController,
} from '@angular/common/http/testing';
import { Observable } from 'rxjs';

describe('Service: Auth', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [AuthService],
    });
  });

  afterEach(() => {
    localStorage.removeItem('token');
  });

  afterEach(inject(
    [HttpTestingController],
    (httpMock: HttpTestingController) => {
      httpMock.verify();
    }
  ));

  fit('Return true when there is a token in localStorage', inject(
    [AuthService],
    (service: AuthService) => {
      const testToken = '12345';
      localStorage.setItem('token', testToken);
      spyOn(service.jwtHelper, 'isTokenExpired').and.returnValue(false);
      expect(service.loggedin()).toBeTruthy();
    }
  ));

  fit('Returns false where there is no token in localStorage', inject(
    [AuthService],
    (service: AuthService) => {
      expect(service.loggedin()).toBeFalsy();
    }
  ));

  fit('Calls http.post to login a user', inject(
    [HttpTestingController, AuthService],
    (httpMock: HttpTestingController, service: AuthService) => {
      const user = { name: 'name' };
      const testToken = '12345';
      const loginParam = { name: 'name', password: 'password' };
      spyOn(service.jwtHelper,'decodeToken').and.returnValue('12345');
      service.login(loginParam).subscribe(() => {
        expect(localStorage.getItem('user')).toBe(JSON.stringify(user));
        expect(localStorage.getItem('token')).toBe(testToken);
        expect(service.jwtHelper.decodeToken).toHaveBeenCalled();
      });

      const req = httpMock.expectOne('http://localhost:5000/v1/auth/login');
      expect(req.request.method).toEqual('POST');

      req.flush({ token: testToken, userForResponse: user });
    }
  ));
});
