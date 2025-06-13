export interface FullTrack {
    name: string;
    id: string;
    artists: SimpleArtist[];
    album: SimpleAlbum;
    previewUrl: string | null;
    durationMs: number;
    popularity: number;
}

export interface SimpleArtist {
    name: string;
    id: string;
}

export interface SimpleAlbum {
    name: string;
    id: string;
    releaseDate: string;
    images: AlbumImage[];
}

export interface AlbumImage {
    url: string;
    height: number;
    width: number;
}