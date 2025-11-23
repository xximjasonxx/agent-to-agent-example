import { useState } from 'react'
import PropTypes from 'prop-types'

function MessageInput({ onSend, isThinking }) {
  const [value, setValue] = useState('')

  const handleSubmit = (event) => {
    event.preventDefault()
    const trimmed = value.trim()
    if (!trimmed) return
    onSend(trimmed)
    setValue('')
  }

  const handleKeyDown = (event) => {
    if (event.key === 'Enter' && !event.shiftKey) {
      event.preventDefault()
      handleSubmit(event)
    }
  }

  return (
    <form className="chat-input" onSubmit={handleSubmit}>
      <div className="chat-input__field-wrapper">
        <textarea
          className="chat-input__field"
          placeholder="Type a message…"
          value={value}
          onChange={(event) => setValue(event.target.value)}
          onKeyDown={handleKeyDown}
          rows={1}
          aria-label="Message"
        />
        <button
          type="submit"
          className="chat-input__button"
          disabled={!value.trim() || isThinking}
        >
          {isThinking ? 'Thinking…' : 'Send'}
        </button>
      </div>
      <p className="chat-input__hint">Press Enter to send · Shift + Enter for a newline</p>
    </form>
  )
}

MessageInput.propTypes = {
  onSend: PropTypes.func.isRequired,
  isThinking: PropTypes.bool,
}

MessageInput.defaultProps = {
  isThinking: false,
}

export default MessageInput
