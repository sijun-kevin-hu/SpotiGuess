import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Track } from '../models/tracks-model';
@Injectable({
  providedIn: 'root'
})
export class SpotifyService {
  constructor(private http: HttpClient) { }

  getTopTracks(): Observable<Track[]> {
    return this.http.get<Track[]>('/api/spotifyapi/top-tracks');
  }
}
