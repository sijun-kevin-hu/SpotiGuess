import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { FullTrack } from '../models/tracks-model';
@Injectable({
  providedIn: 'root'
})
export class SpotifyService {
  constructor(private http: HttpClient) { }

  getTopTracks(
    timeRange: string,
    limit: number,
  ): Observable<FullTrack[]> {
    let params = new HttpParams()
      .set('timeRange', timeRange)
      .set('limit', limit.toString())
    return this.http.get<FullTrack[]>('/api/spotifyapi/top-tracks', { params });
  }
}
