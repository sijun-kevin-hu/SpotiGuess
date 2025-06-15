export interface FullTrack {
    name: string;
    id: string;
    artists: SimpleArtist[];
    album: SimpleAlbum;
    preview_url: string | null;
    duration_ms: number;
    popularity: number;
}

export interface SimpleArtist {
    name: string;
    id: string;
}

export interface SimpleAlbum {
    name: string;
    id: string;
    release_date: string;
    images: AlbumImage[];
}

export interface AlbumImage {
    url: string;
    height: number;
    width: number;
}