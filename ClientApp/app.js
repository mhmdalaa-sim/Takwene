(function () {
  const e = React.createElement;
  const demoTracks = [
    { id: 1, title: "Midnight Atlas", artistName: "Naya Kade", genre: "Alternative", status: "Distributed", isrc: "US-TK1-24-00001", releaseDate: "2024-09-06", artwork: "MA", distributions: [{ id: 11, dspName: "Spotify", status: "Live", submittedAt: "2024-08-20" }, { id: 12, dspName: "Apple Music", status: "Live", submittedAt: "2024-08-20" }, { id: 13, dspName: "YouTube Music", status: "Live", submittedAt: "2024-08-22" }] },
    { id: 2, title: "Paper Moons", artistName: "Lio James", genre: "Electronic", status: "Submitted", isrc: "US-TK1-24-00002", releaseDate: "2024-10-11", artwork: "PM", distributions: [{ id: 21, dspName: "Spotify", status: "Processing", submittedAt: "2024-09-18" }, { id: 22, dspName: "Apple Music", status: "Processing", submittedAt: "2024-09-18" }, { id: 23, dspName: "Deezer", status: "Pending", submittedAt: null }] },
    { id: 3, title: "Cedar & Smoke", artistName: "Mara Sol", genre: "Folk", status: "Draft", isrc: "US-TK1-24-00003", releaseDate: "2024-11-01", artwork: "CS", distributions: [{ id: 31, dspName: "Spotify", status: "Not submitted", submittedAt: null }, { id: 32, dspName: "Apple Music", status: "Not submitted", submittedAt: null }] },
    { id: 4, title: "Afterimage", artistName: "Naya Kade", genre: "R&B", status: "Distributed", isrc: "US-TK1-24-00004", releaseDate: "2024-07-19", artwork: "A", distributions: [{ id: 41, dspName: "Spotify", status: "Live", submittedAt: "2024-07-01" }, { id: 42, dspName: "Tidal", status: "Live", submittedAt: "2024-07-03" }] }
  ];
  const statusColors = { Distributed: ["#d9f3e7", "#12734d"], Submitted: ["#fff0cc", "#9a6200"], Draft: ["#e8edf1", "#5c6873"] };
  const dspColors = { Live: ["#d9f3e7", "#12734d"], Processing: ["#fff0cc", "#9a6200"], Pending: ["#e8edf1", "#5c6873"], "Not submitted": ["#f0e3dc", "#9a4d2d"] };
  const trackStatuses = ['Draft', 'Submitted', 'Distributed'];
  const distributionStatuses = ['Pending', 'Live', 'Rejected'];

  function normalizeTrack(track) {
    const distributions = (track.distributions || track.dspDistributions || []).map(distribution => Object.assign({}, distribution, {
      status: typeof distribution.status === 'number' ? distributionStatuses[distribution.status] : distribution.status,
      submittedAt: distribution.submittedAt && !distribution.submittedAt.startsWith('0001-') ? distribution.submittedAt : null
    }));
    return Object.assign({}, track, {
      status: typeof track.status === 'number' ? trackStatuses[track.status] : track.status,
      artistName: track.artistName || (track.artist && (track.artist.name || track.artist.artistName)) || 'Unknown artist',
      artwork: track.artwork || track.title.substring(0, 2).toUpperCase(),
      distributions: distributions
    });
  }

  function apiJson(path) {
    return fetch(path).then(response => {
      if (!response.ok) throw new Error('API request failed: ' + response.status);
      return response.json();
    });
  }

  function Badge({ label, colors }) { return e('span', { className: 'badge', style: { background: colors[0], color: colors[1] } }, label); }
  function App() {
    const [tracks, setTracks] = React.useState([]);
    const [filter, setFilter] = React.useState('All');
    const [selectedId, setSelectedId] = React.useState(null);
    const [selectedTrack, setSelectedTrack] = React.useState(null);
    const [view, setView] = React.useState('list');
    const [loading, setLoading] = React.useState(true);
    const [apiFallback, setApiFallback] = React.useState(false);
    const visibleTracks = filter === 'All' ? tracks : tracks.filter(track => track.status === filter);
    const selected = selectedTrack || tracks.find(track => track.id === selectedId);

    React.useEffect(() => {
      apiJson('/api/tracks')
        .then(data => {
          const backendTracks = Array.isArray(data) ? data : (data.items || data.tracks || data.data || []);
          setTracks(backendTracks.map(normalizeTrack));
          setApiFallback(false);
        })
        .catch(() => {
          setTracks(demoTracks);
          setApiFallback(true);
        })
        .finally(() => setLoading(false));
    }, []);

    function openTrack(track) {
      setSelectedId(track.id);
      setSelectedTrack(track);
      setView('detail');
      apiJson('/api/tracks/' + encodeURIComponent(track.id))
        .then(data => setSelectedTrack(normalizeTrack(data)))
        .catch(() => {});
    }
    function backToList() { setView('list'); }

    const listView = e('section', null,
      e('div', { className: 'page-heading' }, e('div', null, e('h1', null, 'Tracks'), e('p', null, 'Keep your releases moving from draft to distribution.')), e('div', { className: 'track-count' }, e('strong', null, tracks.length), ' total tracks')),
      e('div', { className: 'toolbar' }, e('div', { className: 'view-tabs' }, e('button', { className: 'tab active' }, 'Track list')), e('label', { className: 'filter-label' }, 'Status', e('select', { value: filter, onChange: event => setFilter(event.target.value) }, e('option', null, 'All'), e('option', null, 'Draft'), e('option', null, 'Submitted'), e('option', null, 'Distributed')))),
      e('div', { className: 'table-wrap' }, e('div', { className: 'table-head' }, e('span', null, 'Track'), e('span', null, 'Artist'), e('span', null, 'Genre'), e('span', null, 'Status'), e('span', null, '')), visibleTracks.map(track => e('button', { className: 'track-row', key: track.id, onClick: () => openTrack(track) }, e('span', { className: 'track-cell' }, e('span', { className: 'cover' }, track.artwork), e('strong', null, track.title)), e('span', null, track.artistName), e('span', { className: 'muted' }, track.genre), e('span', null, e(Badge, { label: track.status, colors: statusColors[track.status] })), e('span', { className: 'arrow' }, '->'))), visibleTracks.length === 0 && e('div', { className: 'empty' }, 'No tracks match this status.'))
    );
    const distributionRows = selected && selected.distributions.map(dsp => e('div', { className: 'dsp-row', key: dsp.id },
      e('div', { className: 'dsp-icon' }, dsp.dspName.charAt(0)),
      e('strong', null, dsp.dspName),
      e('span', { className: 'dsp-date' }, dsp.submittedAt ? 'Submitted ' + new Date(dsp.submittedAt).toLocaleDateString('en-US', { month: 'short', day: 'numeric', year: 'numeric' }) : 'Awaiting submission'),
      e(Badge, { label: dsp.status, colors: dspColors[dsp.status] })
    ));
    const detailContent = selected && e('div', null,
      e('div', { className: 'detail-hero' }, e('div', { className: 'large-cover' }, selected.artwork), e('div', null, e('div', { className: 'eyebrow' }, selected.genre), e('h1', null, selected.title), e('p', null, selected.artistName), e(Badge, { label: selected.status, colors: statusColors[selected.status] })), e('div', { className: 'detail-id' }, e('span', null, 'ISRC'), e('strong', null, selected.isrc))),
      e('div', { className: 'info-grid' }, e('div', { className: 'info-block' }, e('span', null, 'Release date'), e('strong', null, new Date(selected.releaseDate).toLocaleDateString('en-US', { year: 'numeric', month: 'long', day: 'numeric' }))), e('div', { className: 'info-block' }, e('span', null, 'Genre'), e('strong', null, selected.genre))),
      e('div', { className: 'distribution-section' }, e('div', { className: 'section-heading' }, e('div', null, e('h2', null, 'Distribution'), e('p', null, 'Delivery status across your digital platforms.')), e('span', { className: 'platform-count' }, selected.distributions.length + ' platforms')), e('div', { className: 'dsp-list' }, distributionRows))
    );
    const detailView = e('section', { className: 'detail-view' }, e('button', { className: 'back-button', onClick: backToList }, '<- Back to tracks'), detailContent);
    return e('div', { className: 'app-shell' },
      e('header', { className: 'topbar' }, e('div', { className: 'brand' }, e('div', { className: 'brand-mark' }, 'T'), e('span', null, 'takwene')), e('div', { className: 'workspace-name' }, 'CATALOG / TRACKS')),
      e('main', { className: 'main-content' }, e('div', { className: 'eyebrow' }, 'Music catalog'), apiFallback && e('div', { className: 'api-notice' }, 'Backend unavailable. Showing demo data.'), loading ? e('div', { className: 'empty' }, 'Loading tracks...') : view === 'list' ? listView : detailView)
    );
  }

  const style = document.createElement('style');
  style.textContent = `
    :root { --ink:#18232f; --blue:#244b63; --line:#dce4e4; --paper:#fff; --soft:#f4f7f6; --coral:#dc7354; }
    .app-shell { min-height:100vh; background:var(--soft); } .topbar { height:74px; background:#fff; border-bottom:1px solid var(--line); display:flex; align-items:center; justify-content:space-between; padding:0 clamp(24px,6vw,88px); } .brand { display:flex; align-items:center; gap:10px; font:700 21px "Space Grotesk"; letter-spacing:.02em; } .brand-mark { background:var(--coral); color:#fff; width:31px; height:31px; display:grid; place-items:center; border-radius:8px 8px 8px 2px; } .workspace-name,.eyebrow { color:#73818a; font-size:11px; font-weight:700; letter-spacing:.14em; text-transform:uppercase; } .main-content { max-width:1180px; margin:auto; padding:52px clamp(24px,6vw,88px) 80px; } h1,h2 { font-family:"Space Grotesk", sans-serif; margin:0; color:var(--ink); } h1 { font-size:clamp(35px,4vw,53px); letter-spacing:-.04em; } h2 { font-size:22px; } p { color:#71808a; margin:10px 0 0; line-height:1.5; } .page-heading { display:flex; align-items:end; justify-content:space-between; margin:17px 0 42px; } .track-count { color:#73818a; font-size:13px; } .track-count strong { color:var(--ink); font:700 22px "Space Grotesk"; margin-right:7px; } .toolbar { display:flex; align-items:center; justify-content:space-between; border-bottom:1px solid var(--line); margin-bottom:0; } .tab { border:0; border-bottom:2px solid var(--coral); background:none; padding:0 0 15px; color:var(--ink); font-weight:700; } .filter-label { display:flex; align-items:center; gap:12px; color:#73818a; font-size:12px; font-weight:700; text-transform:uppercase; letter-spacing:.08em; } select { background:#fff; border:1px solid var(--line); border-radius:5px; color:var(--ink); padding:9px 34px 9px 12px; font-size:13px; } .table-wrap { background:#fff; border:1px solid var(--line); border-top:0; box-shadow:0 8px 25px rgba(27,48,56,.04); } .table-head,.track-row { display:grid; grid-template-columns:minmax(220px,2.1fr) 1.3fr 1fr 1fr 28px; gap:18px; align-items:center; padding:17px 23px; text-align:left; } .table-head { color:#8a969b; font-size:10px; font-weight:700; letter-spacing:.12em; text-transform:uppercase; background:#fbfcfc; } .track-row { width:100%; border:0; border-top:1px solid #edf1f0; background:#fff; color:#3f4c53; font-size:14px; transition:background .2s; } .track-row:hover { background:#f7faf8; } .track-cell { display:flex; align-items:center; gap:13px; color:var(--ink); } .cover,.large-cover { background:#244b63; color:#d8eee4; display:grid; place-items:center; font:700 13px "Space Grotesk"; } .cover { width:40px; height:40px; border-radius:4px; } .large-cover { width:160px; height:160px; border-radius:8px; font-size:42px; background:#244b63; box-shadow:10px 10px 0 #d7e6df; } .muted,.dsp-date { color:#829096; } .badge { display:inline-block; border-radius:20px; padding:6px 10px; font-size:11px; font-weight:700; white-space:nowrap; } .arrow { color:#9da9ab; font-size:20px; text-align:right; } .empty { padding:40px 24px; color:#73818a; text-align:center; } .detail-view { max-width:900px; } .back-button { border:0; background:transparent; color:#63747c; font-weight:700; padding:0; margin:18px 0 37px; } .detail-hero { display:flex; align-items:center; gap:30px; border-bottom:1px solid var(--line); padding-bottom:42px; } .detail-hero h1 { margin-top:8px; } .detail-hero p { font-size:18px; margin:8px 0 17px; } .detail-id { margin-left:auto; display:flex; flex-direction:column; gap:7px; color:#859399; font-size:11px; letter-spacing:.1em; text-transform:uppercase; } .detail-id strong { color:var(--ink); font:600 13px "DM Sans"; letter-spacing:0; } .info-grid { display:flex; gap:90px; padding:27px 0; border-bottom:1px solid var(--line); } .info-block { display:flex; flex-direction:column; gap:8px; min-width:150px; } .info-block span { color:#859399; font-size:11px; text-transform:uppercase; letter-spacing:.1em; font-weight:700; } .info-block strong { font-size:14px; } .distribution-section { padding-top:41px; } .section-heading { display:flex; justify-content:space-between; align-items:end; margin-bottom:20px; } .section-heading p { font-size:14px; } .platform-count { color:#71808a; font-size:13px; } .dsp-list { border-top:1px solid var(--line); } .dsp-row { display:grid; grid-template-columns:38px 1.3fr 1fr auto; align-items:center; gap:15px; padding:17px 0; border-bottom:1px solid var(--line); font-size:14px; } .dsp-icon { width:32px; height:32px; border-radius:50%; display:grid; place-items:center; background:#e3ece9; color:var(--blue); font:700 13px "Space Grotesk"; }
    @media (max-width:700px) { .workspace-name { display:none; } .main-content { padding-top:35px; } .page-heading { align-items:start; gap:20px; flex-direction:column; margin-bottom:28px; } .table-head { display:none; } .track-row { grid-template-columns:1fr auto; gap:7px 12px; padding:16px; } .track-row > :nth-child(2),.track-row > :nth-child(3) { grid-column:2; font-size:12px; } .track-row > :nth-child(2) { grid-row:1; } .track-row > :nth-child(3) { grid-row:2; } .track-row > :nth-child(4) { grid-column:1; grid-row:2; justify-self:start; margin-left:53px; } .track-row > :nth-child(5) { grid-column:2; grid-row:1 / span 2; } .detail-hero { align-items:flex-start; flex-wrap:wrap; gap:22px; } .large-cover { width:110px; height:110px; } .detail-hero > div:nth-child(2) { flex:1; min-width:170px; } .detail-id { margin-left:0; width:100%; } .dsp-row { grid-template-columns:38px 1fr auto; } .dsp-date { grid-column:2; font-size:12px; } .dsp-row .badge { grid-column:3; grid-row:1 / span 2; } }
  `;
  document.head.appendChild(style);
  ReactDOM.createRoot(document.getElementById('root')).render(e(App));
})();
