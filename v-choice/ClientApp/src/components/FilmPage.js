import React from 'react'
import { CommentsList } from './CommentsList'
// import FilmCard from './FilmCard'
import { NavMenu } from './NavMenu'

function FilmPage() {
	return (
		<div>
			<NavMenu />
			{/* <FilmCard film={ } /> */}
			<CommentsList filmId={11} />
		</div>
	)
}

export default FilmPage
