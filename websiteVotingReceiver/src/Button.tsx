export interface ButtonProps {
    onClick: () => void
    text: string
    disabled?: boolean
}

export const Button = ({onClick, text, disabled}: ButtonProps) => {
    return (
        <button onClick={onClick} disabled={disabled}>{text}</button>
    );
}