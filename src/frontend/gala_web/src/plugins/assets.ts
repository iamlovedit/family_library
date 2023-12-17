import 'katex/dist/katex.min.css'
import '@assets/main.css'
import 'uno.css';
import 'katex/dist/katex.css';
import 'vfonts/Lato.css'
import 'vfonts/FiraCode.css'

function naiveStyleOverride() {
    const meta = document.createElement('meta')
    meta.name = 'naive-ui-style'
    document.head.appendChild(meta)
}

function setupAssets() {
    naiveStyleOverride()
}

export default setupAssets