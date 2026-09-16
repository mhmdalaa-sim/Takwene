(function() {
  const e = React.createElement;
  const root = document.getElementById('root');

  function useState(init) { return React.useState(init); }

  function App() {
    const [tracks, setTracks] = useState([]);
    const [filterStatus, setFilterStatus] = useState('');
    const [selected, setSelected] = useState(null);
    const [token, setToken] = useState('');
    const [artists, setArtists] = useState([]);

    React.useEffect(() => { loadTracks(); loadArtists(); }, []);

    function api(path, opts) {
      opts = opts || {};
      opts.headers = opts.headers || {};
      if (token) opts.headers['Authorization'] = 'Bearer ' + token;
      return fetch(path, opts).then(r => r.json().catch(() => r));
    }

    function loadTracks() {
      const qs = filterStatus ? '?status=' + encodeURIComponent(filterStatus) : '';
      fetch('/api/tracks' + qs).then(r => r.json()).then(setTracks);
    }

    function loadArtists() {
      fetch('/api/artists').then(r => r.json()).then(setArtists);
    }

    function selectTrack(t) {
      fetch('/api/tracks/' + t.id).then(r => r.json()).then(data => setSelected(data));
    }

    async function getToken() {
      const username = prompt('username for demo token', 'demo');
      const password = prompt('password', 'demo');
      const res = await fetch('/api/auth/token',{method:'POST',headers:{'Content-Type':'application/json'},body:JSON.stringify({username,password})});
      const j = await res.json();
      if (j.token) setToken(j.token);
      else alert('token error');
    }

    async function distribute() {
      if (!selected) return;
      const ids = selected.distributions.map(d => d.dspId);
      const res = await fetch('/api/tracks/' + selected.id + '/distribute',{method:'POST',headers:{'Content-Type':'application/json','Authorization':'Bearer '+token},body:JSON.stringify(ids)});
      if (res.status === 204) { alert('Submitted'); loadTracks(); selectTrack(selected); }
      else { const j = await res.json(); alert(JSON.stringify(j)); }
    }

    return e('div', {style:{padding:20}},
      e('h1', null, 'Takwene - Tracks'),
      e('div', null,
        e('button',{onClick:getToken}, 'Get Demo JWT Token'),
        e('span',{style:{marginLeft:10}}, token ? 'Token set' : 'No token')
      ),
      e('div',{style:{display:'flex',marginTop:10}},
        e('div',{style:{flex:1,marginRight:20}},
          e('div',null,
            e('label',null,'Filter by status: '),
            e('select',{value:filterStatus,onChange:ev=>{setFilterStatus(ev.target.value); setTimeout(loadTracks,0)}},
              e('option',{value:''},'All'),
              e('option',{value:'Draft'},'Draft'),
              e('option',{value:'Submitted'},'Submitted'),
              e('option',{value:'Distributed'},'Distributed')
            )
          ),
          e('ul',null, tracks.map(t=> e('li',{key:t.id,style:{cursor:'pointer',padding:6,borderBottom:'1px solid #eee'},onClick:()=>selectTrack(t)},
            e('strong',null,t.title), ' — ', t.artistName, ' — ', t.genre, ' — ', t.status
          )))
        ),
        e('div',{style:{width:420}},
          selected ? e('div',null,
            e('h3',null,selected.title),
            e('div',null,'Artist: ' + selected.artistName),
            e('div',null,'ISRC: ' + selected.isrc),
            e('div',null,'Release: ' + new Date(selected.releaseDate).toLocaleDateString()),
            e('div',null,'Genre: ' + selected.genre),
            e('div',null,'Status: ' + selected.status),
            e('h4',null,'Distributions'),
            e('ul',null, selected.distributions.map(d=> e('li',{key:d.id}, d.dspName + ' — ' + d.status + ' — ' + new Date(d.submittedAt).toLocaleString()))),
            e('div',null,
              e('button',{onClick:distribute},'Distribute to listed DSPs (requires JWT)')
            )
          ) : e('div',null,'Select a track to see details')
        )
      )
    );
  }

  ReactDOM.createRoot(document.getElementById('root')).render(React.createElement(App));
})();
