import { useEffect, useRef } from 'react'
import PropTypes from 'prop-types'

function MessageList({ messages }) {
  const listRef = useRef(null)

  useEffect(() => {
    const el = listRef.current
    if (!el) return
    el.scrollTop = el.scrollHeight
  }, [messages])

  return (
    <section
      ref={listRef}
      className="chat-messages"
      aria-label="Conversation messages"
    >
      <ul className="chat-messages__list">
        {messages.map((msg) => (
          <li
            key={msg.id}
            className={`chat-message chat-message--${msg.sender}`}
          >
            <div className="chat-message__avatar" aria-hidden="true">
              {msg.sender === 'user' ? 'You' : 'AI'}
            </div>
            <div className="chat-message__bubble">
              <div className="chat-message__meta">
                <span className="chat-message__sender">
                  {msg.sender === 'user' ? 'You' : 'Assistant'}
                </span>
                <span className="chat-message__time">{msg.timestamp}</span>
              </div>
              <p className="chat-message__text">{msg.content}</p>
            </div>
          </li>
        ))}
      </ul>
    </section>
  )
}

MessageList.propTypes = {
  messages: PropTypes.arrayOf(
    PropTypes.shape({
      id: PropTypes.number.isRequired,
      sender: PropTypes.oneOf(['user', 'assistant']).isRequired,
      content: PropTypes.string.isRequired,
      timestamp: PropTypes.string.isRequired,
    }),
  ).isRequired,
}

export default MessageList
