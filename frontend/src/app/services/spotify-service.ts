import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { FullTrack } from '../models/tracks-model';
@Injectable({
  providedIn: 'root'
})
export class SpotifyService {
  constructor(private http: HttpClient) { }

  getTopTracks(): Observable<FullTrack[]> {
    return this.http.get<FullTrack[]>('/api/spotifyapi/top-tracks');
  }
}
