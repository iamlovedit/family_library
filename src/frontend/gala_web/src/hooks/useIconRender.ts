import { h } from 'vue'
import SvgIcon from '@components/SvgIcon/Index.vue'

export function useIconRender() {
    interface IconConfig {
        icon: string
        color?: string
        size?: number
    }

    interface IconStyle {
        color?: string
        size?: number
    }

    function renderIcon(config: IconConfig) {
        const { icon, color, size } = config;
        const style: IconStyle = {};
        if (color) {
            style.color = color;
        }
        if (size) {
            style.size = size
        }

        return h(SvgIcon, {
            icon,
            style
        })
    }
    return {
        renderIcon
    }
}