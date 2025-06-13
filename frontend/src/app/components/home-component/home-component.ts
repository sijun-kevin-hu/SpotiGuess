import { Component, OnInit } from '@angular/core';
import { SpotifyService } from '../../services/spotify-service';
import { FullTrack } from '../../models/tracks-model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-home-component',
  imports: [CommonModule],
  templateUrl: './home-component.html',
  styleUrl: './home-component.css'
})
export class HomeComponent {
  tracks: FullTrack[] = [];

  constructor(private spotifyService: SpotifyService) {}

  ngOnInit(): void {
    this.spotifyService.getTopTracks().subscribe({
      next: (data) => {
        this.tracks = data;
        console.log(data);
      },
      error: (err) => console.error('Error fetching tracks:', err)
    });
  }
}
